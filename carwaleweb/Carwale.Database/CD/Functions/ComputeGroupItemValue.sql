IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[ComputeGroupItemValue]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [CD].[ComputeGroupItemValue]
GO

	-- =============================================
-- Author:		Amit Verma
-- Create date: 17 March 2013
-- Description:	Saves or deletes ItemValue for group items
--Modified by : Reshma on 26-04-2013
-- SELECT [CD].[ComputeGroupItemValue]( 2200,288)
-- =============================================
/*
	Changes History:
       
       Edited By               		EditedON               		Description
       ----------------       -----------------              	-----------------------
       
*/
CREATE FUNCTION [CD].[ComputeGroupItemValue]
(
	@VersionID int,
	@IMID int
)
RETURNS varchar(max)
AS
BEGIN
 --declare @VersionID int = 2200
 --declare @IMID int = 288
 declare @Template varchar(max) = (select Top 1 Template from CD.GroupItemConfig WITH(NOLOCK) where ItemMasterID = @IMID)
 declare @ItemIDs varchar(max) = (select Top 1 ItemIDs from CD.GroupItemConfig WITH(NOLOCK) where ItemMasterID = @IMID)
 declare @UnitCode varchar(max) = (select Top 1 UnitCode from CD.GroupItemConfig WITH(NOLOCK) where ItemMasterID = @IMID)
 declare @Rules varchar(max) = (select Top 1 Rules from CD.GroupItemConfig WITH(NOLOCK) where ItemMasterID = @IMID)
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
 declare @Unit table
	    (
           id TINYINT IDENTITY,
           UnitCode bit
        )
 INSERT INTO @Items (ItemMasterId) SELECT items FROM [dbo].[SplitText](@ItemIDs,',')
 INSERT INTO @ItemValue (Value,Unit,ItemMasterId)
      select CONVERT(VARCHAR,COALESCE(CustomText,convert(varchar,ItemValue),UD.Name)),UT.Name,I.ItemMasterId
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
 INSERT INTO @Unit (UnitCode)
	  SELECT Data FROM [dbo].[StringSplitNew](@UnitCode,',')
	  
declare @countOrg int = (select COUNT(id) from @ItemValue)
declare @count int = (select COUNT(id) from @ItemValue)
while(@count != 0)
begin
	set @Val = (select Top 1 Value from @ItemValue)
	if ((select Top 1 UnitCode from @Unit) = 1)
	begin
		set @Val += '~ ' + (select Top 1 Unit from @ItemValue) + '~'
	end
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
	delete top(1) from @Unit
	set @count = (select COUNT(id) from @ItemValue)
end
end
IF (@Rules IS NOT NULL OR @Rules != '')
BEGIN
	SET @Value = (SELECT [dbo].[GetSubstring](@Value,@Rules))
END

DECLARE @LEN INT = LEN(@Value)-1

IF(RIGHT(@Value,1)=',')
SET @Value=LEFT(@Value,@LEN)

return @Value

END
