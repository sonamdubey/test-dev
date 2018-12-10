IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SplitTextByTwoDelimitersWithFourColumns]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[SplitTextByTwoDelimitersWithFourColumns]
GO

	



CREATE FUNCTION [dbo].[SplitTextByTwoDelimitersWithFourColumns](@String varchar(6000),@FirstDelimit char(1),@SecondDelimit char(1))       
RETURNS @tbl TABLE (Val1 varchar(25) ,Val2 varchar(25),Val3 varchar(25),Val4 varchar(25)) 
AS
BEGIN      
DECLARE 
  @keyPair VARCHAR(100),
  @FirstID VARCHAR(25),
  @SecondID VARCHAR(25),
  @ThirdID VARCHAR(25),
  @FourthID VARCHAR(25),
  @FirstDelimiter char(1),
  @SecondDelimiter char(1)

DECLARE @SourceStr VARCHAR(6000)
SET @SourceStr = @String
SET @FirstDelimiter=@FirstDelimit
SET @SecondDelimiter=@SecondDelimit

DECLARE
  @len INT,
  @pos INT,
  @found INT,
  @tempstr1 VARCHAR(100),
  @tempstr2 VARCHAR(100)

SELECT
  @len = LEN(@SourceStr),
  @pos = 1

SET @SourceStr = @SourceStr + @FirstDelimiter

/* Find the first instance of a comma */
SET @found = CHARINDEX(@FirstDelimiter, @SourceStr, @pos)

WHILE @found > 0
BEGIN  

  /* The key pair starts at the @pos position and goes to the @found position minus the @pos position   */
  SET @keyPair= SUBSTRING(@SourceStr, @pos, @found - (@pos))

  /* Double-check that '.' exists to avoid failure */
  /* If no '.' exists, assume value is modelid    */
  IF CHARINDEX(@SecondDelimiter,@keyPair) = 0
  BEGIN
    SET @FirstID = NULLIF(@keyPair, '')
    SET @SecondID = NULL
  END
  ELSE
  BEGIN
    /* @FirstID is everything left of the '.'  @SecondID is everything on the right */
    SET @FirstID = NULLIF(SUBSTRING(@keyPair, 1, 
        CHARINDEX(@SecondDelimiter, @keyPair) - 1), '')
	SET	@tempstr1 = NULLIF(SUBSTRING(@keyPair,CHARINDEX(@SecondDelimiter, @keyPair) + 1, LEN(@keyPair)),'')
    SET @SecondID = NULLIF(SUBSTRING(@tempstr1, 1, CHARINDEX(@SecondDelimiter, @tempstr1) - 1),'')
	SET	@tempstr2 = NULLIF(SUBSTRING(@tempstr1,CHARINDEX(@SecondDelimiter, @tempstr1) + 1, LEN(@tempstr1)),'')
	SET @ThirdID = NULLIF(SUBSTRING(@tempstr2, 1, CHARINDEX(@SecondDelimiter, @tempstr2) - 1),'')
	SET	@FourthID = NULLIF(SUBSTRING(@tempstr2,CHARINDEX(@SecondDelimiter, @tempstr2) + 1, LEN(@tempstr2)),'')
	INSERT @tbl(Val1 ,Val2 ,Val3 ,Val4)  values(@FirstID,@SecondID,@ThirdID,@FourthID)
        
  END

 --IF  @pos>1
 --   SET @TargetStr=@TargetStr+ @FirstDelimiter+cast(@SecondID as varchar(10))-- @FirstID if want first parameter also
 --ELSE SET @TargetStr=@TargetStr+ cast(@SecondID as varchar(10))
  

  /* Move to the next position and search again */
  SET @pos = @found + 1
  SET @found = CHARINDEX(@FirstDelimiter, @SourceStr, @pos)
   

END
return 

END

