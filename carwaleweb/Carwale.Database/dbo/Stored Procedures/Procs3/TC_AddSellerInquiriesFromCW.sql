IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_AddSellerInquiriesFromCW]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_AddSellerInquiriesFromCW]
GO

	-- Created By:	Binumon George
-- Create date: 20 Feb 2012
-- Description:	Adding sell inquiry from CW to TC
-- AM modified 22-Feb-2012 To check if SellInquiryId already exists for DealerId
-- AM modified 22-July-2012 To push profile id to TC_AddSellerInquiry(By Avishkar)
-- Modified By :Tejashree Patil 22-Oct-2012 assign @InquiryStatus=NULL instead of CSI.StatusId	
-- Modified By :Tejashree Patil 25-Jan-2013 Commented AddTcInquiries Sp related code
-- =============================================
CREATE  Procedure [dbo].[TC_AddSellerInquiriesFromCW]          
@BranchId INT, 
@SellInquiryId INT
AS
BEGIN
    DECLARE @cnt tinyint
    
    SELECT SellInquiryId FROM AP_DealerPackageInquiries WHERE DealerId=@BranchId AND SellInquiryId=@SellInquiryId
     
    IF @@ROWCOUNT = 0
    --  AM modified 22-Feb-2012 To check if SellInquiryId already exists for DealerId 
    BEGIN

		INSERT INTO AP_DealerPackageInquiries(DealerId, SellInquiryId, SendDate)
					 VALUES(@BranchId, @SellInquiryId, GETDATE())
					 
		--	Commented By :Tejashree Patil 25-Jan-2013
		/*		 
		--declare @BranchId			NUMERIC,
		DECLARE @VersionId			INT, @CustomerName		VARCHAR(50),	@Mobile	VARCHAR(15),	@Email				VARCHAR(100),
		@Location	VARCHAR(50),	@MakeYear			DATETIME, 	@Price	BIGINT, 	@Kilometers			int, 
		@Color				VARCHAR(100),		@AdditionalFuel		VARCHAR(50),	@RegNo VARCHAR(40),	@RegistrationPlace	VARCHAR(40),
		@Insurance			VARCHAR(40),	@InsuranceExpiry	datetime,	@Owners				VARCHAR(20),	@CarDriven			VARCHAR(20),
		@Tax				VARCHAR(20),	@CityMileage		VARCHAR(20),	@Accidental			BIT	,	@FloodAffected		BIT,	
		@InteriorColor		VARCHAR(100),	@Comments			VARCHAR(500),	@UserId BIGINT,	@InquiryStatus SMALLINT,
		@InquirySource SMALLINT,	@profileId BIGINT	
		 
		 --getting all inquiry for i/p customer and adding id into @TblPurIn
		Select @profileId= CSI.Id , @CustomerName=CD.Name, @Mobile=CD.Mobile , @Email=CD.EMail,
		@VersionId=CSI.CarVersionId,@MakeYear= CSI.MakeYear,@Price=CSI.Price , @Kilometers=CSI.Kilometers ,@Color=Csi.Color,@AdditionalFuel=csd.AdditionalFuel,
		@RegNo=csi.CarRegNo,@RegistrationPlace=csd.RegistrationPlace,@Insurance=csd.Insurance,@InsuranceExpiry=csd.InsuranceExpiry,@Owners=csd.Owners,@CarDriven=csd.CarDriven,
		@Tax=csd.Tax,@CityMileage=csd.CityMileage,@Accidental=csd.Accidental,@FloodAffected=csd.FloodAffected,@InteriorColor=csd.InteriorColor,
		@Comments=csi.Comments,	@InquiryStatus=NULL	-- Modified By :Tejashree Patil 22-Oct-2012 assign @InquiryStatus=NULL instead of CSI.StatusId	
		From AP_DealerPackageInquiries  AS DPI 
		JOIN  CustomerSellInquiries AS CSI on DPI.SellInquiryId = CSI.Id  
		--JOIN vwMMV AS CV on CSI.CarVersionId = CV.VersionId  
		JOIN Customers AS CD on CSI.CustomerId = CD.Id 
		--LEFT JOIN CustomerSellInquiriesValuation AS CSV  on CSV.SellInquiryId = CSI.ID 
		JOIN CustomerSellInquiryDetails AS CSD on CSD.InquiryId = CSI.ID 
		where CSI.Id  =@SellInquiryId
		
		SET @InquiryStatus=NULL
		EXECUTE TC_AddSellerInquiry @BranchId,@VersionId,@CustomerName,@Email,@Mobile,@Location,NULL,NULL,@MakeYear,@Price,@Kilometers,@Color,@AdditionalFuel,@RegNo,@RegistrationPlace,@Insurance,@InsuranceExpiry,@Owners,@CarDriven,@Tax,@CityMileage,@Accidental,@FloodAffected,@InteriorColor,null,@UserId,@InquiryStatus,1,@ProfileId		
		*/
	END
END
