IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dv].[CheckDuplicateCity]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dv].[CheckDuplicateCity]
GO

	-- =============================================
-- Author:		Manish
-- Create date: 26-12-2012
-- Description:	Duplicate city name exists in cities table
-- =============================================
CREATE PROCEDURE dv.CheckDuplicateCity
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    CREATE TABLE #temp
    (
		StateId smallint,
		City varchar(50)
    )
    
    INSERT INTO #temp(StateId,City)
	select stateid,name 
	from Cities 
	where IsDeleted=0 
	group by Name,StateId 
	having COUNT(*)>1	
	
	
	SELECT *
	FROM #temp
	
	DROP TABLE #temp
 
END
