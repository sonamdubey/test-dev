IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetRequesterAndWarrantyDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetRequesterAndWarrantyDetails]
GO

	-- =============================================
-- Author:		Yuga Hatolkar
-- Create date: 2nd July, 2015
-- Description:	To Get Requester And Warranty Details.
-- Modified By : Kartik Rathod on Oct 5,2015 (Fetched SoldDealerId)
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_GetRequesterAndWarrantyDetails]
	@AbSure_CarDetailsId BIGINT,
	@DispStatus	INT
AS
BEGIN
	DECLARE @Status INT,@CancelReason VARCHAR(250),@CancelledReason VARCHAR(250),@RegNumber VARCHAR(50)
	SELECT @CancelledReason = Reason FROM AbSure_ReqCancellationReason WITH(NOLOCK) WHERE Id = 7;
	
	SELECT  @Status = ACD.Status,@CancelReason = ACD.CancelReason,@RegNumber = ACD.RegNumber
            FROM    AbSure_CarDetails ACD WITH(NOLOCK)
            WHERE   ACD.Id = @AbSure_CarDetailsId


	IF @DispStatus = 2
	BEGIN
		SELECT ACD.OwnerName AS OwnerName, ACD.OwnerPhoneNo AS OwnerPhoneNo, ACD.OwnerAddress AS OwnerAddress, ACD.OwnerEmail AS OwnerEmail, --ACD.PolicyNo AS PolicyNo,
		AWT.Warranty AS WarrantyType, C.Name AS City, A.Name AS Area, ACD.AppointmentDate AS AppointmentDate, ACD.AppointmentTime AS Slot, AP.DealerId AS VerificationDealerId
		FROM AbSure_CarDetails ACD WITH(NOLOCK)
		LEFT JOIN AbSure_WarrantyTypes AWT WITH (NOLOCK) ON ACD.AbSure_WarrantyTypesId = AWT.AbSure_WarrantyTypesId
		LEFT JOIN Cities C WITH (NOLOCK) ON ACD.OwnerCityId = C.ID
		LEFT JOIN Areas A WITH (NOLOCK) ON ACD.OwnerAreaId = A.ID
		LEFT JOIN AbSure_WarrantyActivationPending AP WITH (NOLOCK) ON AP.AbSure_CarDetailsId = ACD.Id AND AP.IsActive = 1
		WHERE ACD.Id = @AbSure_CarDetailsId
	END

	ELSE
	BEGIN
	
		IF(@Status = 3 AND @CancelReason = @CancelledReason and @DispStatus = 1)
		BEGIN
			SELECT @AbSure_CarDetailsId = dbo.Absure_GetMasterCarId(@RegNumber,@AbSure_CarDetailsId)
		END

		SELECT AW.CustName AS CustName, AW.Address AS CustAddress, AW.Mobile AS CustNumber, AW.Email AS CustEmail, AW.WarrantyStartDate AS WarrantyStartDate,
		AW.WarrantyEndDate AS WarrantyEndDate, AWT.Warranty	AS WarrantyType, ACD.PolicyNo AS PolicyNo, AW.Kilometer AS Kilometer, AW.EngineNo AS EngineNo, AW.ChassisNo AS ChassisNo,
		AW.DealerId SoldDealerId
		FROM AbSure_ActivatedWarranty AW WITH (NOLOCK)
		LEFT JOIN AbSure_CarDetails ACD WITH (NOLOCK) ON ACD.Id = AW.AbSure_CarDetailsId
		LEFT JOIN AbSure_WarrantyTypes AWT WITH (NOLOCK) ON AW.WarrantyTypeId = AWT.AbSure_WarrantyTypesId
		WHERE ACD.Id = @AbSure_CarDetailsId
	END
END
