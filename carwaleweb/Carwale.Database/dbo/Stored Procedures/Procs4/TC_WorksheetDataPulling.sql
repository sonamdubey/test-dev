IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_WorksheetDataPulling]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_WorksheetDataPulling]
GO

	-- Modified By	:	Surendra
-- Mod	date	:	7th May,2012
-- Description	:	Inquiry assignment to user is now based in tc_task table N also if user is using Worksheet only feature then 
--					All inquiries will push to logged in user according to their roles
-- =============================================
-- Author		:	Surendra
-- Create date	:	30th Jan,2012
-- Description	:	Data pulling in worksheet
-- TC_WorksheetDataPulling 98,5,1
-- =============================================
CREATE PROCEDURE [dbo].[TC_WorksheetDataPulling]
@UserId BIGINT,-- Logged In User Id
@BranchId BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;	
	DECLARE @AccessLimit INT=3 -- User has access limit to hadle maximum fresh inquiry
	DECLARE @UnHandledCount INT  -- User hasn't taken action on these inquiries and already in pool
	DECLARE @IsWorksheetOnly BIT=0
	
	SELECT @AccessLimit=ISNULL(freshLeadCount,3),@IsWorksheetOnly=isWorksheetOnly FROM TC_DealerConfiguration 
	WHERE DealerId=@BranchId
	
	-- Checkin how many pending lead user have 
	IF(@IsWorksheetOnly=0) -- Dealer is using only worksheet
	BEGIN
		SELECT @UnHandledCount =COUNT(*) FROM TC_InquiriesLead L	
		WHERE L.TC_UserId=@UserId AND L.IsActionTaken=0  -- Pending inquiries
		
		IF(@UnHandledCount>=@AccessLimit)  -- means pending leads> permissiable leads to handle
		BEGIN
			RETURN 0
		END	
			
		BEGIN TRY
			BEGIN TRANSACTION
			
				-- creating table variable to insert unassigned lead id from that Branch later all operation are 
				-- done based on these lead id
				DECLARE @tblLeadId TABLE(LeadId int)
										
				INSERT INTO @tblLeadId(LeadId) SELECT TOP (@AccessLimit-@UnHandledCount) TC_InquiriesLeadId FROM (SELECT TC_InquiriesLeadId, ROW_NUMBER() OVER( PARTITION BY TC_InquiriesLeadId ORDER BY  InquiryDate DESC) AS rownum 
					FROM  vwTC_Inquiries V INNER JOIN TC_Tasks T ON V.TC_InquiryTypeId=T.TC_InquiryTypeId
					INNER JOIN TC_RoleTasks RT ON T.Id=RT.TaskId 
					INNER JOIN TC_Users U ON RT.RoleId=U.RoleId AND U.Id=@UserId						
					WHERE  V.TC_UserId IS NULL AND V.BranchId=@BranchId ) AS tbl WHERE rownum=1
										
				-- assigning input user to selected lead	
				UPDATE TC_InquiriesLead SET TC_UserId=@UserId					
				WHERE TC_InquiriesLeadId IN(SELECT LeadId FROM @tblLeadId) AND BranchId=@BranchId AND TC_UserId IS NULL
				
			COMMIT TRANSACTION
		END TRY
				
		BEGIN CATCH
					ROLLBACK TRAN
					SELECT ERROR_NUMBER() AS ErrorNumber;
		END CATCH;	
			
		RETURN @AccessLimit-@UnHandledCount
	END	
	ELSE
	BEGIN
		-- assigning all permitted lead to logged in user 		
		DECLARE @tblLeadIdAll TABLE(LeadId int)
										
		INSERT INTO @tblLeadIdAll(LeadId) SELECT TC_InquiriesLeadId FROM (SELECT TC_InquiriesLeadId, ROW_NUMBER() OVER( PARTITION BY TC_InquiriesLeadId ORDER BY  InquiryDate DESC) AS rownum 
			FROM  vwTC_Inquiries V INNER JOIN TC_Tasks T ON V.TC_InquiryTypeId=T.TC_InquiryTypeId
			INNER JOIN TC_RoleTasks RT ON T.Id=RT.TaskId 
			INNER JOIN TC_Users U ON RT.RoleId=U.RoleId AND U.Id=@UserId						
			WHERE  V.TC_UserId IS NULL AND V.BranchId=@BranchId ) AS tbl WHERE rownum=1
								
		-- assigning input user to selected lead	
		UPDATE TC_InquiriesLead SET TC_UserId=@UserId					
		WHERE TC_InquiriesLeadId IN(SELECT LeadId FROM @tblLeadIdAll) AND BranchId=@BranchId AND TC_UserId IS NULL
		
		
		RETURN 1
	END		
END

