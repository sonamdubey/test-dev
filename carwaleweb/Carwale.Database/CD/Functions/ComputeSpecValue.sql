IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[ComputeSpecValue]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [CD].[ComputeSpecValue]
GO

	-- =============================================
-- Author:		Amit Verma
-- Create date: 17 March 2013
-- Description:	Saves or deletes ItemValue for group items
-- SELECT [CD].[ComputeSpecValue]( 1012,28)
-- =============================================
/*
	Changes History:
       
       Edited By               		EditedON               		Description
       ----------------       -----------------              	-----------------------
       
*/
CREATE FUNCTION [CD].[ComputeSpecValue]
(
	@VersionID int,
	@ID INT
	--@IMID int
)
RETURNS varchar(max)
AS
BEGIN
 --declare @VersionID int = 2200
 --declare @ID int = 50
 declare @Template varchar(max)-- = (select Top 1 Template from dbo.New_Old_Specs_mapping WITH(NOLOCK) where ID = @ID)
 declare @ItemIDs varchar(max)-- = (select Top 1 ItemIDs from dbo.New_Old_Specs_mapping WITH(NOLOCK) where ID = @ID)
 DECLARE @ItemID int
 select Top 1 @Template = Template, @ItemIDs = ItemIDs, @ItemID = ItemMasterID from dbo.New_Old_Specs_mapping WITH(NOLOCK) where ID = @ID
 declare @Value varchar(max) = ''
 declare @Val varchar(max) = null
 declare @Items table
	    (
           id TINYINT IDENTITY,
           ItemMasterId int
        )
 declare @ItemValue table
	    (
           id TINYINT IDENTITY,
           ItemMasterId int,
           Value varchar(max),
           Unit varchar(max)
        )
 declare @TString table
	    (
           id TINYINT IDENTITY,
           String varchar(max)
        )
 INSERT INTO @Items (ItemMasterId) SELECT items FROM [dbo].[SplitText](@ItemIDs,',')
 INSERT INTO @ItemValue (Value,Unit,ItemMasterId)
      select CONVERT(VARCHAR(200),COALESCE(CustomText
      ,CASE
		WHEN IV.DataTypeId = 2 AND IV.ItemValue = 1 THEN IM.Name
		WHEN IV.DataTypeId = 2 AND IV.ItemValue = 0 THEN NULL
		ELSE convert(varchar,ItemValue) END
      ,UD.Name)),UT.Name,I.ItemMasterId
      --select ISNULL(CustomText,'')+ISNULL(CAST(ItemValue AS VARCHAR(20)),'')+ISNULL(UD.Name,''),UT.Name,I.ItemMasterId
      FROM @Items I
      left join CD.ItemValues IV WITH(NOLOCK) on I.ItemMasterId = IV.ItemMasterId 
      and iv.CarVersionId = @VersionID
      left join CD.UserDefinedMaster UD WITH(NOLOCK) on IV.UserDefinedId = UD.UserDefinedId
      left join CD.ItemMaster IM WITH(NOLOCK) on IV.ItemMasterId = IM.ItemMasterId
      left join CD.UnitTypes UT WITH(NOLOCK) on IM.UnitTypeId = UT.UnitTypeId --and IV.ItemMasterId in( SELECT items FROM [dbo].[SplitText](@ItemIDs,','))
      order by I.id asc
IF ((select count(id) from @ItemValue) = (select count(items) FROM [dbo].[SplitText](@ItemIDs,',')))
begin
 INSERT INTO @TString (String)
	  SELECT Data FROM [dbo].[StringSplitNew](@Template,'~')
	  
declare @countOrg int = (select COUNT(id) from @ItemValue)
declare @count int = (select COUNT(id) from @ItemValue)
while(@count != 0)
begin
	set @Val = (select Top 1 Value from @ItemValue)
	delete top(1) from @ItemValue
	IF ((SELECT TOP 1 Value FROM @ItemValue) IS NOT NULL or @countOrg > 2 or @count = 1)
	BEGIN
		SET @Val = REPLACE((select Top 1 String from @TString),'|',@Val)
	END
	IF(@Val IS NOT NULL)
	BEGIN
		SET @Value += @Val
	END
	delete top(1) from @TString
	set @count = (select COUNT(id) from @ItemValue)
end
end
IF(@ItemID = 30) /*Seating Capacity*/
BEGIN
	IF (@Value = 'CVT')
		SET @Value = NULL
END

IF(@ItemID = 9) /*Seating Capacity*/
BEGIN
	IF (ISNUMERIC(@Value) = 0 AND @Value LIKE '%&%')
	BEGIN
		SET @Value = REPLACE(@Value,' ','')
		SET @Value = (SELECT TOP 1 Data FROM [dbo].[StringSplitNew](@Value,'&') ORDER BY Data DESC)
	END
END


DECLARE @LEN INT = LEN(@Value)-1

IF(RIGHT(@Value,1)=',')
SET @Value=LEFT(@Value,@LEN)

return @Value

END




