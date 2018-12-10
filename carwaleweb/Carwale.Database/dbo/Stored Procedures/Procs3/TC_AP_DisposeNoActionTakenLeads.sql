IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_AP_DisposeNoActionTakenLeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_AP_DisposeNoActionTakenLeads]
GO

	-- =============================================  
-- Author:      <Khushaboo Patil>  
-- Create date: <10/09/2015>  
-- Description: <Dispose Leads which are active but no action taken in 60 days>  
-- ============================================= 
CREATE PROCEDURE [dbo].[TC_AP_DisposeNoActionTakenLeads]
AS
BEGIN
		DECLARE  @tblInquiriesLead TABLE (Id INT IDENTITY,LeadId BIGINT,UserId BIGINT)
		BEGIN TRY
			--BEGIN TRANSACTION AP_DisposeNoActionTakenLeads
			--INSERT ALL LEADID WHICH ARE ACTIVE BUT NO ACTION TAKEN IN 60 DAYS
			-- LEADS WHICH ARE SCHEDULED BUT NO ACTION TAKEN IN 60 DAYS
			INSERT INTO @tblInquiriesLead (LeadId,UserId)
			SELECT  AC.TC_LeadId,AC.TC_UsersId 
			FROM TC_ActiveCalls AC WITH(NOLOCK)
			WHERE DATEDIFF(DD,CONVERT(DATE,ScheduledOn),CONVERT(DATE,GETDATE())) > 60 

			UNION
			-- LEADS WHICH ARE IN POOL MORE THAN 60 DAYS
			SELECT  TL.TC_LeadId, TU.Id
			FROM TC_Lead TL WITH(NOLOCK)
				INNER JOIN TC_Users TU WITH(NOLOCK) ON TU.Id = 
				(SELECT TOP 1 TU1.Id FROM TC_Users TU1 WITH(NOLOCK)	WHERE TU1.BranchId = TL.BranchId AND TU1.IsActive = 1 ORDER BY TU1.Id ASC)
			WHERE TC_LeadStageId IS NULL AND LeadVerifiedBy IS NULL AND 
			DATEDIFF(DD,CONVERT(DATE,LeadCreationDate),CONVERT(DATE,GETDATE())) > 60   
			ORDER BY TC_LeadId
			

			-- DISPOSE LEADS AND INQUIRIES 
			DECLARE @RowCnt INT = 0
			DECLARE @RowId INT = 1 
			DECLARE @LeadId BIGINT
			DECLARE @UserId BIGINT
			DECLARE @NextFolloupDate AS DATETIME = GETDATE()

			INSERT INTO TC_AP_ArchivedLeads
			SELECT DISTINCT LeadId,UserId, GETDATE() FROM @tblInquiriesLead
			SET @RowCnt = @@ROWCOUNT
			
			WHILE(@RowId <= @RowCnt)
			BEGIN
				SELECT @LeadId = LeadId,@UserId=UserId FROM @tblInquiriesLead WHERE Id = @RowId
				EXEC TC_CallScheduling @LeadId,@UserId,41,'Archived Lead',@NextFolloupDate,41
				SET @RowId = @RowId + 1
			END
			
			--COMMIT TRANSACTION AP_DisposeNoActionTakenLeads

		END TRY
		BEGIN CATCH
			--ROLLBACK TRANSACTION AP_DisposeNoActionTakenLeads

			INSERT INTO TC_Exceptions(Programme_Name,TC_Exception,TC_Exception_Date)
			VALUES('TC_AP_DisposeNoActionTakenLeads', (ERROR_MESSAGE()+'ERROR_NUMBER(): '+CONVERT(VARCHAR,ERROR_NUMBER())),GETDATE())

		END CATCH		
END






