IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TEMPTC_AddSellerInquiryProcessSellersInquiries]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TEMPTC_AddSellerInquiryProcessSellersInquiries]
GO

	CREATE procedure [dbo].[TEMPTC_AddSellerInquiryProcessSellersInquiries]
(
@DealerId BIGINT
)
as
declare  @tblCustomer table(id int identity(1,1),SellInquiryId Bigint)
	insert into @tblCustomer(SellInquiryId)
	Select distinct CSI.ID as SellInquiryId  -- AM Added 22-Feb-2012 "distinct" To check if SellInquiryId already exists for DealerId
		From AP_DealerPackageInquiries  AS DPI 
		JOIN  CustomerSellInquiries AS CSI on DPI.SellInquiryId = CSI.Id  	   
	Where  DPI.DealerId = @DealerId 
	
declare @rowCount smallint,@loopCount smallint=1
select @rowCount=COUNT(*) from @tblCustomer

declare @LeadId BIGINT

while @rowCount>=@loopCount 

begin
	declare @SellInquiryId int
	select @SellInquiryId=SellInquiryId from @tblCustomer	WHERE ID=@loopCount
	
	declare @BranchId			NUMERIC,
	@VersionId			INT, @CustomerName		VARCHAR(50),	@Mobile	VARCHAR(15),	@Email				VARCHAR(100),
	@Location	VARCHAR(50),	@MakeYear			DATETIME, 	@Price	BIGINT, 	@Kilometers			int, 
	@Color				VARCHAR(100),		@AdditionalFuel		VARCHAR(50),	@RegNo VARCHAR(40),	@RegistrationPlace	varchar(40),
	@Insurance			varchar(40),	@InsuranceExpiry	datetime,	@Owners				varchar(20),	@CarDriven			varchar(20),
	@Tax				varchar(20),	@CityMileage		varchar(20),	@Accidental			bit	,	@FloodAffected		bit,	
	@InteriorColor		varchar(100),	@Comments			VARCHAR(500),	@UserId BIGINT,	@InquiryStatus SMALLINT,
	@InquirySource SMALLINT,	@profileId varchar(10),@CreatedDate datetime	, @cityid int
	
	set @BranchId=@DealerId
	
	 --getting all inquiry for i/p customer and adding id into @TblPurIn
	Select @profileId= 'S' + Convert(Varchar,CSI.Id) , @CustomerName=CD.Name, @Mobile=CD.Mobile , @Email=CD.EMail,
	@VersionId=CV.VersionId,@MakeYear= CSI.MakeYear,@Price=CSI.Price , @Kilometers=CSI.Kilometers ,@Color=Csi.Color,@AdditionalFuel=csd.AdditionalFuel,
	@RegNo=csi.CarRegNo,@RegistrationPlace=csd.RegistrationPlace,@Insurance=csd.Insurance,@InsuranceExpiry=csd.InsuranceExpiry,@Owners=csd.Owners,@CarDriven=csd.CarDriven,
	@Tax=csd.Tax,@CityMileage=csd.CityMileage,@Accidental=csd.Accidental,@FloodAffected=csd.FloodAffected,@InteriorColor=csd.InteriorColor,
	@Comments=csi.Comments,	@InquiryStatus=CSI.StatusId,@CreatedDate=DPI.SendDate,@cityid=CSI.CityId	
	From AP_DealerPackageInquiries  AS DPI 
	JOIN  CustomerSellInquiries AS CSI on DPI.SellInquiryId = CSI.Id and DPI.DealerId=@DealerId 
	JOIN vwMMV AS CV on CSI.CarVersionId = CV.VersionId  
	JOIN Customers AS CD on CSI.CustomerId = CD.Id
	JOIN CustomerSellInquiryDetails AS CSD on CSD.InquiryId = CSI.ID 
	where CSI.Id  =@SellInquiryId
		
	EXECUTE TEMPTC_AddSellerInquiry @BranchId,@VersionId,@CustomerName,@Mobile,@Email,@Location,@MakeYear,@Price,@Kilometers,@Color,@AdditionalFuel,@RegNo,@RegistrationPlace,@Insurance,@InsuranceExpiry,@Owners,@CarDriven,@Tax,@CityMileage,@Accidental,@FloodAffected,@InteriorColor,@Comments,@UserId,@InquiryStatus,@InquirySource,@ProfileId,@cityid,@CreatedDate
	set @loopCount=@loopCount+1
end



