IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[DCRM].[GetDealerPlanPrint]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [DCRM].[GetDealerPlanPrint]
GO

	




-- Description	:	Get Dealer ID, Name, Plan, Expirydate
-- Author		:	Dilip V. 19-Mar-2012
-- Modifier		:	Dilip V. 20-Mar-2012(Added @Address)
--					Dilip V. 20-Mar-2012(Added @Area,@City,@PinCode,@State)
CREATE PROCEDURE [DCRM].[GetDealerPlanPrint]
	@DealerID		NUMERIC(18,0),
	@DealerName		VARCHAR(100) OUTPUT,
	@Organization	VARCHAR(60) OUTPUT,
	@Address		VARCHAR(100) OUTPUT,
	@Area			VARCHAR(50) OUTPUT,
	@City			VARCHAR(50) OUTPUT,
	@PinCode		VARCHAR(6) OUTPUT,
	@State			VARCHAR(30) OUTPUT,
	@ProdTypeID		INT OUTPUT,
	@ExpiryDate		DATETIME OUTPUT,
	@ContactPerson	VARCHAR(60) OUTPUT
AS
BEGIN
	SET NOCOUNT ON			

	SELECT @DealerName = D.FirstName + ' '+ D.LastName ,@ExpiryDate = CCP.ExpiryDate,@ContactPerson = D.ContactPerson,
	@Organization = D.Organization,@Address=D.Address1,@Area = A.Name,@City = VWC.City,@State = VWC.State,@PinCode = D.Pincode,
	@ProdTypeID =	(SELECT TOP 1 CPR.PackageId
						FROM ConsumerPackageRequests CPR
						WHERE CPR.ConsumerType = 1
						AND CPR.isApproved = 1
						AND CPR.IsActive = 1
						AND CPR.ConsumerId = D.ID
						ORDER BY CPR.EntryDate DESC
					)
	FROM Dealers D
	INNER JOIN Areas A ON D.AreaId = A.ID
	INNER JOIN vwCity VWC ON VWC.CityId = D.CityId
	INNER JOIN ConsumerCreditPoints CCP ON CCP.ConsumerId = D.ID
	INNER JOIN InquiryPointCategory IPC ON IPC.ID = CCP.PackageType
	LEFT JOIN DCRM_ADM_UserDealers DAUD ON DAUD.DealerId = D.ID AND DAUD.RoleId = 4
	WHERE D.ID = @DealerID 
	AND CCP.ConsumerType = 1
	
END




