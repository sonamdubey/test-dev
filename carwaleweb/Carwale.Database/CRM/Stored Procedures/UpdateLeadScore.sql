IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[UpdateLeadScore]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[UpdateLeadScore]
GO

	-- =============================================
-- Author:		Reshma Shetty
-- Create date: 03/07/2013
-- Description:	Insert the Lead score into tables CRM.LS_PQScore and CRM.LSLeadCategoryScore and update it in CRM_Leads table
-- Chetan -Added LeadScoreVersion and added update operation in crm_leads 
--Chetan Dev - Added Score in PQ_clientinfo for each PqId on 25-11-2013
-- =============================================
CREATE PROCEDURE [CRM].[UpdateLeadScore] 
	-- Add the parameters for the stored procedure here
	@LeadCategoryScore LeadScore READONLY,
	@LeadScoreVersion SMALLINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE 

	@LeadId NUMERIC,
	@PQId NUMERIC
    -- Insert statements for procedure here
    
	SELECT @LeadId = LS.LeadId  FROM @LeadCategoryScore LS 
	WHERE LS.CategoryId IS NUll

	SELECT @PQId = LS.PQId  FROM @LeadCategoryScore LS 
	WHERE LS.CategoryId IS NUll

--Insert the score lead category wise into the [CRM].[LSLeadCategoryScore]
	INSERT INTO [CRM].[LSLeadCategoryScore]
           ([LeadId]
           ,[CBDId]
           ,[PQ_Id]
           ,[CategoryId]
           ,[LeadScore]
           ,[CreatedOn])
	SELECT LeadId,
		CBDId,
		PQId,
		CategoryId,
		Score,
		GETDATE()
	FROM @LeadCategoryScore
	WHERE CategoryId IS NOT NULL
	
--Insert the score PQ wise into the [CRM].[LS_PQScore]
	INSERT INTO [CRM].[LS_PQScore]
           ([LeadId]
           ,[CBD_Id]
           ,[PQ_Id]
           ,[Score]
           ,[CreatedOn])
    SELECT LeadId,
		CBDId,
		PQId,
		Score,
		GETDATE()
	FROM @LeadCategoryScore
	WHERE CategoryId IS NULL
	
	IF(@LeadId <> -1)

	BEGIN
--Update the latest score into the CRM_Leads table
	UPDATE CL
	SET LeadScore=LS.Score
	FROM CRM_Leads CL
	INNER JOIN @LeadCategoryScore LS ON LS.LeadId=CL.ID AND LS.CategoryId IS NULL
	
	UPDATE CL
	SET CL.LeadScoreVersion =@LeadScoreVersion
	FROM CRM_Leads CL WHERE CL.ID = @LeadId
	
	END 

	UPDATE PQ
	SET PQ.LeadScore=LS.Score
	FROM PQ_ClientInfo PQ
	INNER JOIN @LeadCategoryScore LS ON LS.PQId=PQ.PQId AND LS.CategoryId IS NULL

	UPDATE PQ
	SET PQ.LeadScoreVersion =@LeadScoreVersion
	FROM PQ_ClientInfo PQ WHERE PQ.PQId = @PQId

	
    
END
