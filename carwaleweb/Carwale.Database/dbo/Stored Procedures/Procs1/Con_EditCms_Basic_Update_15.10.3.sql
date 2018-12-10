IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_EditCms_Basic_Update_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_EditCms_Basic_Update_15]
GO

	
 --Modified By:Prashant Vishe On 15 Nov 2013
 --Modification:added parameter IsFeatured

 --Modified By:Vinay Kumar On  8thj April 2014
 --Modification:added parameter @SocialMediaLine,@IsCompatibleNews
 --Modified by : Piyush Sahu - Added PhotoCredit Field 20 oct 2015
CREATE PROCEDURE [dbo].[Con_EditCms_Basic_Update_15.10.3]           
@Title varchar(250),       
@DisplayDate datetime,          
@AuthorName varchar(100),      
@AuthorId NUMERIC(18,0),          
@Description varchar(8000),       
@LastUpdatedBy numeric(18,0),        
@LastUpdatedTime datetime,      
@SubCatId varchar(2000),      
@ID numeric(18,0),    
@CFId VarChar(200),    
@ValType VarChar(200),    
@Value VarChar(2000),    
@ExtdInfoId VarChar(200),
@IsSticky bit = NULL,
@StickyFromDate datetime = NULL,
@StickyToDate datetime  = NULL,
@IsFeatured bit,
@SocialMediaLine VARCHAR(120)=null,
@IsCompatibleNews bit,
@PhotoCredit VARCHAR(250)=null
     
AS          
BEGIN          
          

 delete from Con_EditCms_BasicSubCategories where BasicId = @ID 

 UPDATE Con_EditCms_Basic          
 SET        
 Title = @Title,   
 DisplayDate = @DisplayDate,        
 AuthorName = @AuthorName,      
 AuthorId  = @AuthorId,      
 Description = @Description,      
 LastUpdatedBy = @LastUpdatedBy,      
 LastUpdatedTime = @LastUpdatedTime, 
 IsSticky = @IsSticky,
 StickyFromDate = @StickyFromDate,
 StickyToDate = @StickyToDate,
 IsFeatured=@IsFeatured,
 IsCompatibleForNewsLetter=@IsCompatibleNews,
 SocialMediaLine=@SocialMediaLine,    
 PhotoCredit =@PhotoCredit   
 WHERE        
 ID = @ID
         
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
  if @CFId <> ''    
	 Begin     
	  --Declare @boolVal Bit = Null      
	  --Declare @numericVal Numeric = Null       
	  --Declare @decimalVal Decimal = Null       
	  --Declare @textVal VarChar(250) = Null      
	  --Declare @dateTimeVal DateTime = Null      
        
	  --if @ValType = 1      
	  -- Set @boolVal = CONVERT(bit, @Value)      
	  --if @ValType = 2      
	  -- Set @numericVal = CONVERT(numeric, @Value)       
	  --if @ValType = 3      
	  -- Set @decimalVal = CONVERT(decimal, @Value)      
	  --if @ValType = 4      
	  -- Set @textVal = CONVERT(varchar, @Value)      
	  --if @ValType = 5      
	  -- Set @dateTimeVal = CONVERT(datetime, @Value)    
          
	  if @ExtdInfoId Is Not Null      
		  Begin    
		   if @ExtdInfoId = ''    
			   Begin    
				--Insert Into Con_EditCms_OtherInfo (BasicId, CategoryFieldId, BooleanValue, NumericValue, DecimalValue, TextValue, DateTimeValue, ValueType, LastUpdatedTime, LastUpdatedBy)       
				--Values (@ID, @CFId, @boolVal, @numericVal, @decimalVal, @textVal, @dateTimeVal, @ValType,@LastUpdatedTime, @LastUpdatedBy)      
				Insert Into Con_EditCms_OtherInfo (BasicId, CategoryFieldId,TextValue, ValueType, LastUpdatedTime, LastUpdatedBy)        
				 Select @ID As BasicId, Id, Value, ValType, @LastUpdatedTime As LastUpdatedTime, @LastUpdatedBy As LastUpdatedBy From dbo.Split(@CFId, @ValType, @Value, '|')    
			   End    
		   Else    
			   Begin    
								--Update Con_EditCms_OtherInfo Set BooleanValue = @boolVal, NumericValue = @numericVal, DecimalValue = @decimalVal, TextValue = @textVal,     
    --DateTimeValue = @dateTimeVal, LastUpdatedTime = @LastUpdatedTime, LastUpdatedBy = @LastUpdatedBy Where ID = @ExtdInfoId    
        
				Declare @TempTable Table    
				 (    
				 Id Int,    
				 Value VarChar(2000),    
				 ValType Int    
				 )    
				 Insert Into @TempTable (Id, Value, ValType) Select Id, Value, ValType From dbo.Split( @ExtdInfoId, @ValType, @Value, '|')    
         
				Update Con_EditCms_OtherInfo    
					Set TextValue = (Select Value From @TempTable tt Where tt.id = Con_EditCms_OtherInfo.id)    
					Where Id in (Select Id From @TempTable)    
        
			   End    
		  End    
	  End     
         
END   
  


