IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDealerDeatilsByActualDealerId_V]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDealerDeatilsByActualDealerId_V]
GO

	

-- =============================================
-- Author: Satish Sharma
-- Create date: 07/08/2014
-- Description:	Get dealer Details based on dealerId
-- exec [dbo].[GetDealerDeatilsByActualDealerId]
--modified by rohan sapkal , fetch address with city from Dealer_NewCar instead of Dealers (if available) 11-05-2015
-- =============================================
CREATE PROCEDURE [dbo].[GetDealerDeatilsByActualDealerId_V.15.5.1] 
	@ActualDealerId INT
	,@DealerName VARCHAR(30) OUTPUT
	,@DealerMobile VARCHAR(50) OUTPUT
	,@DealerEmail VARCHAR(250) OUTPUT
	,@DealerActualMobile VARCHAR(100) OUTPUT
	,@DealerLeadBusinessType INT OUTPUT
	,@DealerAddress VARCHAR(MAX) OUTPUT	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	--SELECT DealerId,DealerName,Phone AS PhoneNo,DealerEmailId AS DealerEmail
	--FROM PQ_DealerSponsored WITH(NOLOCK)
	--WHERE CityId= @CityId and ModelId=@ModelId and IsActive = 1
	SELECT @DealerName = dl.Organization
		,@DealerMobile = dl.ActiveMaskingNumber
		,@DealerEmail = dl.EmailId
		,@DealerActualMobile = dl.MobileNo
		,@DealerLeadBusinessType = dl.DealerLeadBusinessType --modified by ashish verma
		,@DealerAddress = isnull(dl.Address1, ' ') + isnull(dl.Address2, ' ')+ isnull(' '+C.Name, ' ')
	FROM Dealers dl WITH (NOLOCK)
	LEFT JOIN Cities C WITH(NOLOCK) on dl.CityId =C.ID
	WHERE dl.Id = @ActualDealerId

	DECLARE @TempAddress VARCHAR(1000)=NULL

	SELECT @TempAddress = DN.Address + ' ' + C.Name
	FROM Dealer_NewCar DN WITH(NOLOCK) 
	LEFT JOIN Cities C WITH(NOLOCK) ON DN.CityId=C.ID
	WHERE DN.TcDealerId=@ActualDealerId

	IF(@TempAddress<>NULL) SET @DealerAddress=@TempAddress
END

