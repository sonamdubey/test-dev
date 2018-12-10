IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_GetDoubtfulCases]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_GetDoubtfulCases]
GO

	
-- =============================================
-- Author		:	 Yuga Hatolkar
-- Create date	:    15th Sept, 2015
-- Description	:	 To Get all Doubtful Cases.
-- =============================================
CREATE PROCEDURE [dbo].[Absure_GetDoubtfulCases] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;   

      SELECT Id, Reason FROM AbSure_DoubtfulReasons WITH(NOLOCK) 
	  WHERE IsActive = 1

END



