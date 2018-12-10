IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetMaskingNumbersForDealer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetMaskingNumbersForDealer]
GO

	-- =============================================
-- Author:		Ashish G. Kamble	
-- Create date: 14 july 2016
-- Description:	Proc to get the assigned and unassigned masking numbers
-- Modified By : Sunil M. Yadav On 26th Aug 2016, Get Masking no. based on stateId as well.
-- Modified By : Komal Manjare on 14-Sept-2016 get serviceprovider from MM_ServiceProvider
-- Modified By : Vaibhav K 10 Oct 2016
-- =============================================
CREATE PROCEDURE [dbo].[GetMaskingNumbersForDealer] 
	-- Add the parameters for the stored procedure here
	 @DealerId INT,
	 @IsAssigned BIT = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @CityId INT,@StateId INT;

	SELECT @CityId = D.CityId , @StateId = C.StateId
	FROM Dealers D WITH(NOLOCK) 
	JOIN Cities C WITH(NOLOCK) ON C.ID = D.CityId
	WHERE D.ID = @DealerId;

	SELECT MN.MaskingNumber AS Number, ServiceProvider, MSP.ServiceProviderName AS Provider, 1 As IsAssigned
	,Mobile MappedNumbers, TC_InquirySourceId , MN.ExpiryDate, ProductTypeId,MN.MM_SellerMobileMaskingId AS SellerMobileMaskingId
	FROM MM_SellerMobileMasking MN WITH(NOLOCK)
	INNER JOIN Dealers D WITH(NOLOCK) ON D.ID = MN.ConsumerId
	JOIN MM_ServiceProviders MSP (NOLOCK) ON MSP.Id=MN.ServiceProvider -- Komal Manjare on 14-Sept-2016
	WHERE ConsumerId = @DealerId
	
	UNION ALL
	
	SELECT MaskingNumber AS Number, ServiceProvider,MSP.ServiceProviderName AS Provider, 0 As IsAssigned 
	, NULL MappedNumbers, NULL TC_InquirySourceId , NULL ExpiryDate, NULL ProductTypeId,NULL SellerMobileMaskingId
	FROM MM_AvailableNumbers MAN WITH(NOLOCK) 
	JOIN MM_ServiceProviders MSP (NOLOCK) ON MSP.Id=MAN.ServiceProvider -- Komal Manjare on 14-Sept-2016
	WHERE @IsAssigned = 0 AND (CityID = @CityId OR StateId = @StateId)
END

