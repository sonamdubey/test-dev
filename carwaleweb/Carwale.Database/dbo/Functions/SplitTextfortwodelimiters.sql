IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SplitTextfortwodelimiters]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[SplitTextfortwodelimiters]
GO

	CREATE FUNCTION [dbo].[SplitTextfortwodelimiters](@String varchar(8000),@FirstDelimit char(1),@SecondDelimit char(1))       
returns varchar(8000)
as 
BEGIN      
DECLARE 
  @keyPair VARCHAR(1000),
  @FirstID VARCHAR(1000),
  @SecondID VARCHAR(1000),
  @TargetStr VARCHAR(1000),
  @FirstDelimiter char(1),
  @SecondDelimiter char(1)

DECLARE @SourceStr VARCHAR(1000)
SET @SourceStr = @String
SET @TargetStr=''
SET @FirstDelimiter=@FirstDelimit
SET @SecondDelimiter=@SecondDelimit

DECLARE
  @len INT,
  @pos INT,
  @found INT

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
    SET @SecondID = NULLIF(SUBSTRING(@keyPair, 
        CHARINDEX(@SecondDelimiter, @keyPair) + 1, LEN(@keyPair) - 1), '')
  END

 IF  @pos>1
    SET @TargetStr=@TargetStr+ @FirstDelimiter+cast(@SecondID as varchar(10))-- @FirstID if want first parameter also
 ELSE SET @TargetStr=@TargetStr+ cast(@SecondID as varchar(10))
  

  /* Move to the next position and search again */
  SET @pos = @found + 1
  SET @found = CHARINDEX(@FirstDelimiter, @SourceStr, @pos)
   

END
return @TargetStr

END