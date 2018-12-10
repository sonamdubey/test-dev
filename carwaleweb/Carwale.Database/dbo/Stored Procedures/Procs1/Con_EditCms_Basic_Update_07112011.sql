IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_EditCms_Basic_Update_07112011]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_EditCms_Basic_Update_07112011]
GO

	  
CREATE PROCEDURE [dbo].[Con_EditCms_Basic_Update_07112011]       
@Title varchar(250),   
@Url varchar(200),     
@DisplayDate datetime,      
@AuthorName varchar(100),  
@AuthorId NUMERIC(18,0),      
@Description varchar(8000),   
@LastUpdatedBy numeric(18,0),    
@LastUpdatedTime datetime,  
@SubCatId varchar(2000),  
@ID numeric(18,0),
@CFId numeric(18,0),
@ValType numeric(18,0),
@Value varchar(250),
@ExtdInfoId numeric(18,0)      
AS      
BEGIN      
      
 UPDATE Con_EditCms_Basic      
 SET    
 Title = @Title,   
 Url   = @Url ,  
 DisplayDate = @DisplayDate,    
 AuthorName = @AuthorName,  
 AuthorId  = @AuthorId,  
 Description = @Description,  
 LastUpdatedBy = @LastUpdatedBy,  
 LastUpdatedTime = @LastUpdatedTime  
  
 WHERE    
 ID = @ID    
  if @ID>0 and LEN(@SubCatId)>0 and @SubCatId is NOT Null  
    Begin  
      delete from Con_EditCms_BasicSubCategories where BasicId = @ID  
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
			   
		if @ExtdInfoId Is Not Null
		Begin
			if @ExtdInfoId = 0
			Begin
				Insert Into Con_EditCms_OtherInfo (BasicId, CategoryFieldId, BooleanValue, NumericValue, DecimalValue, TextValue, DateTimeValue, ValueType, LastUpdatedTime, LastUpdatedBy)   
				Values (@ID, @CFId, @boolVal, @numericVal, @decimalVal, @textVal, @dateTimeVal, @ValType,@LastUpdatedTime, @LastUpdatedBy)  
			End
			Else
			Begin
				Update Con_EditCms_OtherInfo Set BooleanValue = @boolVal, NumericValue = @numericVal, DecimalValue = @decimalVal, TextValue = @textVal, 
				DateTimeValue = @dateTimeVal, LastUpdatedTime = @LastUpdatedTime, LastUpdatedBy = @LastUpdatedBy Where ID = @ExtdInfoId
			End
		End
    End 
     
END  