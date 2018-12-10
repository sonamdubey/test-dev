IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[GetColoursByVersion]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CD].[GetColoursByVersion]
GO

	
CREATE PROCEDURE [CD].[GetColoursByVersion]
	@Versions VARCHAR(100)
	-- [CD].[GetColoursByVersion] '2340,1254,2200'
AS
  BEGIN
	DECLARE @TempCarVersionID TABLE
        (
           ID INT
        )
    INSERT INTO @TempCarVersionID (id) (SELECT items FROM [dbo].[SplitText](@Versions,','))
    select * from @TempCarVersionID
    
    WHILE ( (SELECT Count(*)
               FROM   @TempCarVersionID) != 0 )
	BEGIN
		SELECT Color, HexCode, CarVersionId FROM VersionColors WHERE IsActive=1 and CarVersionID in (SELECT top 1 id from @TempCarVersionID) Order By HexCode
		DELETE TOP (1) FROM @TempCarVersionID
	END
  END
