IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_WorksheetDataPulling_12Apr]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_WorksheetDataPulling_12Apr]
GO

	

-- =============================================
-- Modified By	:	Surendra
-- Mod	date	:	19th March,2012
-- Description	:	ORDER BY L.CreatedDate ASC added and InquiryDate ASC(before it was desc)
-- =============================================
-- Author		:	Surendra
-- Create date	:	30th Jan,2012
-- Description	:	Data pulling in worksheet
-- TC_WorksheetDataPulling 98,5,1
-- =============================================
CREATE PROCEDURE [dbo].[TC_WorksheetDataPulling_12Apr]
@UserId BIGINT,-- Logged In User Id
@BranchId BIGINT,
@InqType SMALLINT -- 1=Buyer,2=Seller,-1=Both
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.	
	SET NOCOUNT ON;	
	DECLARE @AccessLimit TINYINT=2 -- User has access limit to hadle maximum fresh inquiry
	DECLARE @UnHandledCount TINYINT  -- User hasn't taken action on these inquiries and already in pool
	
	IF(@InqType=-1) -- User has Buyer as well as Seller Permission
		BEGIN
			SELECT @UnHandledCount =COUNT(*) FROM TC_InquiriesLead L	
			WHERE L.TC_UserId=@UserId AND L.IsActionTaken=0 
		END
	ELSE -- User Hase either Buyer or Seller permission
		BEGIN
			SELECT @UnHandledCount =COUNT(*) FROM TC_InquiriesLead L	
			WHERE L.TC_UserId=@UserId AND L.IsActionTaken=0 AND L.TC_InquiryTypeId=@InqType	
		END		
	
	IF(@UnHandledCount<@AccessLimit) -- User can take more fresh inquiries for pool
		BEGIN
			IF(@InqType=-1)
			BEGIN
				-- since here user has both permission(can handle Buyer and Seller) hence updating(assigning) fresh both type of lead
				-- and fetching same lead to display in Workssheet
				BEGIN TRY
					BEGIN TRANSACTION
						DECLARE @tblLeadId1 TABLE(LeadId int)
						INSERT INTO @tblLeadId1(LeadId) SELECT TOP (@AccessLimit-@UnHandledCount) L.TC_InquiriesLeadId FROM 
							TC_InquiriesLead L WHERE L.BranchId=@BranchId AND L.TC_UserId IS NULL 
							ORDER BY L.CreatedDate ASC
							
						UPDATE TC_InquiriesLead SET TC_UserId=@UserId					
						WHERE TC_InquiriesLeadId IN(SELECT LeadId FROM @tblLeadId1) AND BranchId=@BranchId
							
							SELECT TOP (@AccessLimit-@UnHandledCount) * FROM (SELECT TC_InquiriesLeadId, CustomerName,Mobile,Email,InquiryCount,
								 CONVERT(varchar, InquiryDate, 106) AS InquiryDate, CarName,InquiryType,TC_InquiryTypeId,SourceId,TC_InquiryStatusId,TC_InquiriesFollowupActionId,
								 CONVERT(varchar, NextFollowUpDate, 106) as NextFollowUpDate ,IsActionTaken,Status,TC_CustomerId, ROW_NUMBER() OVER( PARTITION BY TC_InquiriesLeadId ORDER BY  InquiryDate ASC) AS rownum 
								FROM  vwTC_Inquiries  WHERE  TC_UserId =@UserId AND TC_InquiriesLeadId IN(SELECT LeadId FROM @tblLeadId1)) AS tbl WHERE rownum=1
					COMMIT TRANSACTION
				END TRY
							
				BEGIN CATCH
							ROLLBACK TRAN
							SELECT ERROR_NUMBER() AS ErrorNumber;
				END CATCH;
			END
		ELSE
			BEGIN
				-- since here user has one type permission(either Buyer or Seller) hence updating(assigning) fresh any type of lead
				-- and fetching same lead to display in Workssheet
				BEGIN TRY
					BEGIN TRANSACTION
						DECLARE @tblLeadId2 TABLE(LeadId int)
						INSERT INTO @tblLeadId2(LeadId) SELECT TOP (@AccessLimit-@UnHandledCount) L.TC_InquiriesLeadId FROM 
							TC_InquiriesLead L WHERE L.BranchId=@BranchId AND L.TC_UserId IS NULL AND L.TC_InquiryTypeId=@InqType	
							ORDER BY L.CreatedDate ASC
							
						UPDATE LD SET TC_UserId=@UserId
						FROM TC_InquiriesLead LD INNER JOIN @tblLeadId2 L ON LD.TC_InquiriesLeadId=L.LeadId	AND LD.BranchId=@BranchId						
							
						SELECT TOP (@AccessLimit-@UnHandledCount) * FROM ( SELECT  TC_InquiriesLeadId, CustomerName,Mobile,Email,InquiryCount,
							 CONVERT(varchar, InquiryDate, 106) AS InquiryDate , CarName,InquiryType,TC_InquiryTypeId,SourceId,TC_InquiryStatusId,TC_InquiriesFollowupActionId,
							 CONVERT(varchar, NextFollowUpDate, 106) as NextFollowUpDate ,IsActionTaken,Status,TC_CustomerId, ROW_NUMBER() OVER( PARTITION BY TC_InquiriesLeadId ORDER BY  InquiryDate ASC) AS rownum 
							FROM  vwTC_Inquiries  WHERE  TC_UserId =@UserId AND TC_InquiryTypeId=@InqType AND TC_InquiriesLeadId IN(SELECT LeadId FROM @tblLeadId2) ) AS tbl WHERE rownum=1
							
					COMMIT TRANSACTION
				END TRY
							
				BEGIN CATCH
							ROLLBACK TRAN
							SELECT ERROR_NUMBER() AS ErrorNumber;
				END CATCH;		
			END	
	END	
END

	
	
