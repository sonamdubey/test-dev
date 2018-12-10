IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_GetExecutivePerformance_Log]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_GetExecutivePerformance_Log]
GO

	-- =============================================
-- Author:		Reshma Shetty
-- Create date: 20/08/2013
-- Description:	Used to log day wise performers into CRM_FLCDailyPerformers table
-- =============================================
CREATE PROCEDURE [dbo].[CRM_GetExecutivePerformance_Log] 
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    DECLARE @Table TABLE (Id INT IDENTITY(1,1),GroupType SMALLINT)
    DECLARE @Counter SMALLINT
    DECLARE @GroupType SMALLINT
    
--Fetch the active group types in the benchmark table to find the performers for each group separately
	INSERT INTO @Table(GroupType)
	SELECT DISTINCT GroupType 
	FROM CRM_ADM_GroupBenchMark CGB
	INNER JOIN CRM_ADM_FLCGroups CF ON CF.Id=CGB.GroupId
	WHERE CGB.IsActive=1
	
	SET @Counter=@@ROWCOUNT
	
--Insert the performers for each group one after the other
	WHILE(@Counter>0)
	BEGIN
	SELECT @GroupType=GroupType FROM @Table WHERE Id=@Counter
	EXEC CRM_GetExecutivePerformance @GroupType
	SET @Counter=@Counter-1
	END
	
	
END
