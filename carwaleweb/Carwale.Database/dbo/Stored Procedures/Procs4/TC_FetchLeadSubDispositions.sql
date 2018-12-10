IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_FetchLeadSubDispositions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_FetchLeadSubDispositions]
GO

	
-- =============================================
-- Author:		<Author,,Vivek Gupta>
-- Create date: <Create Date,28-12-2015,>
-- Description:	<Description,Fetching LeadSubDisposition,>
-- [dbo].[TC_FetchLeadSubDispositions] 4,65
-- =============================================
CREATE PROCEDURE [dbo].[TC_FetchLeadSubDispositions]
@BranchId INT,
@TC_LeadDispositionId SMALLINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   DECLARE @ApplicationId TINYINT
   SET @ApplicationId = (SELECT TOP 1 ApplicationId FROM Dealers WITH(NOLOCK) WHERE Id = @BranchId)

   SELECT TC_LeadSubDispositionId, SubDispositionName 
   FROM TC_LeadSubDisposition WITH(NOLOCK) 
   WHERE 
		TC_LeadDispositionId = @TC_LeadDispositionId 
   AND (
			(@ApplicationId = 1 AND ISNULL(IsVisibleCW,0) = 1) 
		OR  
			(@ApplicationId = 2 AND ISNULL(IsVisibleBW,0) = 1)
		)
   ORDER BY SubDispositionName
   


END



