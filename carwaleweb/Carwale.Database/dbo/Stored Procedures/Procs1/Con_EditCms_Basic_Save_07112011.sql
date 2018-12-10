IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_EditCms_Basic_Save_07112011]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_EditCms_Basic_Save_07112011]
GO

	    
CREATE PROCEDURE [dbo].[Con_EditCms_Basic_Save_07112011]      
@CategoryId numeric(18, 0),      
@Title varchar(250),      
@DisplayDate datetime,      
@AuthorName varchar(100),      
@AuthorId NUMERIC(18,0),    
@Description varchar(8000),      
@LastUpdatedBy NUMERIC(18,0),    
@LastUpdatedTime datetime,    
@Url varchar(200),     
@EnteredBy NUMERIC(18,0),    
@EntryDate datetime,    
@SubCatId varchar(2000),    
@CFId numeric(18,0),
@ValType numeric(18,0),
@Value varchar(250),
@ID numeric(18,0) out      
AS      
BEGIN      
      
 INSERT INTO Con_EditCms_Basic      
 (CategoryId,Title,Url, DisplayDate,AuthorName,authorid, Description, LastUpdatedTime, LastUpdatedBy,EnteredBy,EntryDate,IsActive )      
 VALUES      
 (@CategoryId,@Title,@Url,@DisplayDate,@AuthorName,@AuthorId, @Description, @LastUpdatedTime, @LastUpdatedBy,@EnteredBy,@EntryDate,1 )      
      
 SET @ID = SCOPE_IDENTITY()      
  if @ID>0 and LEN(@SubCatId)>0 and @SubCatId is NOT Null    
    Begin    
      Declare @idx int    
      declare @StrTemp varchar(2000)    
      set @idx =1    
      While @idx != 0    
        begin    
          Set @idx = CharIndex( ',', @SubCatId )    
    if @idx != 0           
   set @StrTemp = Left( @SubCatId, @idx - 1)           
    else           
   set @StrTemp = @SubCatId     
    if LEN(@StrTemp)>0     
    Begin     
      Insert into Con_EditCms_BasicSubCategories (BasicId ,SubCategoryId )    
      values (@ID,@StrTemp )      
    End     
    Set @SubCatId = Right( @SubCatId, Len( @SubCatId ) - @idx )      
        end     
    End    
      
    Declare @isSinglePage Bit  
    Set @isSinglePage = 0  
    Select @isSinglePage = IsSinglePage From Con_EditCms_Category Where Id = @CategoryId  
      
    if @isSinglePage = 1  
    Begin  
	  Declare @pStatus Int  
	  exec dbo.Con_EditCms_ManagePages -1, @ID,'Content', 1, 1, @LastUpdatedBy, @pStatus Out    
    End  
      
    if @CFId != 0 
    Begin
		Declare @boolVal Bit = Null
		Declare @numericVal Numeric = Null 
		Declare @decimalVal Decimal = Null 
		Declare @textVal VarChar(250) = Null
		Declare @dateTimeVal DateTime = Null
		
		if @ValType = 1
			Set @boolVal = CONVERT(bit, @Value)
		if @ValType = 2
			Set @numericVal = CONVERT(numeric, @Value) 
		if @ValType = 3
			Set @decimalVal = CONVERT(decimal, @Value)
		if @ValType = 4
			Set @textVal = CONVERT(varchar, @Value)
		if @ValType = 5
			Set @dateTimeVal = CONVERT(datetime, @Value) 
		
		Insert Into Con_EditCms_OtherInfo (BasicId, CategoryFieldId, BooleanValue, NumericValue, DecimalValue, TextValue, DateTimeValue, ValueType, LastUpdatedTime, LastUpdatedBy) 
		Values (@ID, @CFId, @boolVal, @numericVal, @decimalVal, @textVal, @dateTimeVal, @ValType,@LastUpdatedTime, @LastUpdatedBy)
    End
END 