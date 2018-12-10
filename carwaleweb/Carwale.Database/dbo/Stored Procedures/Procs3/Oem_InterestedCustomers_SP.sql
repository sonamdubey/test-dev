IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Oem_InterestedCustomers_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Oem_InterestedCustomers_SP]
GO

	-- =============================================  
-- Author:  Satish Sharma  
-- Create date: 9/9/2009  
-- Description: To capture details(from OEM website) of the customers who are interested in buying cars  
-- =============================================  
CREATE PROCEDURE [dbo].[Oem_InterestedCustomers_SP]  
 -- Add the parameters for the stored procedure here  
 @MakeId  INT, @CustomerName  VarChar(50),  
 @Email  VarChar(100), @Phone  VarChar(15),  
 @Mobile  VarChar(15), @VersionId  Int,  
 @CityId  nchar(10), @EntryDate  DateTime,  
 @Source VarChar(50), @ModelName VarChar(50),  
 @ModelId Numeric, @VersionName VarChar(50),
 @DefaultVersion Int, @FuelType VarChar(20),
 @FuelTypeId SmallInt, @Transmission VarChar(20),
 @TransmissionId SmallInt,  
 @utm_source VarChar(200)=null,  
 @utm_medium VarChar(200)=null,  
 @utm_content VarChar(200)=null,  
 @utm_campaign VarChar(200)=null,  
 @Price Numeric, @CurrentId Numeric Output  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    -- Insert statements for procedure here  
   
   IF @VersionId = -1
   Begin
		Select @VersionId = ID From CarVersions Where CarModelId = @ModelId And CarFuelType = @FuelTypeId And CarTransmission = @TransmissionId
		IF (@VersionId = -1) OR (ISNULL(@VersionId, 0) = 0)
		Begin
			Select @VersionId = @DefaultVersion
		End
   End 
   INSERT INTO   
   Oem_InterestedCustomers  
   (MakeId, CustomerName, Email, Phone, Mobile,   
    CityId, VersionId, EntryDate, Source, ModelName, FuelType, Transmission,
    ModelId, VersionName, Price,utm_source,utm_medium,utm_content,utm_campaign  
    )  
 Values  
  (@MakeId, @CustomerName, @Email, @Phone, @Mobile,   
   @CityId, @VersionId, @EntryDate, @Source, @ModelName,  @FuelType, @Transmission,
   @ModelId, @VersionName, @Price,@utm_source,@utm_medium,@utm_content,@utm_campaign  
  )   
     
  SET @CurrentId =  SCOPE_IDENTITY()  
END  
  
  