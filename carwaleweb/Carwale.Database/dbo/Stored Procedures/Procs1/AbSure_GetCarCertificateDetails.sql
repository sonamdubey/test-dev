IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetCarCertificateDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetCarCertificateDetails]
GO

	-- =============================================
-- Author	  :	Yuga Hatolkar
-- Create date: 23rd June, 2015
-- Description:	Gets Details required to display on Absure Car Certificate.
-- EXEC AbSure_GetCarCertificateDetails 368
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_GetCarCertificateDetails]

	@AbSure_CarDetailsId BIGINT
	--@IsAgency BIT OUTPUT
AS
DECLARE

	@SurveyorId INT,
	@IsAgency BIT,
	@AgencyId INT	

BEGIN
		SET NOCOUNT ON;

		SELECT @IsAgency = TU.IsAgency FROM AbSure_CarDetails ACD WITH(NOLOCK)
		LEFT JOIN AbSure_CarSurveyorMapping ACSM ON ACD.Id = ACSM.AbSure_CarDetailsId
		LEFT JOIN TC_Users TU ON ACSM.TC_UserId = TU.Id 
		WHERE ACD.Id = @AbSure_CarDetailsId AND TU.IsAgency = 0
		
		SELECT TU.UserName AS SurveyorName, TU.Id, TU.Email AS LoginId, C.Name AS City, ACD.AbSure_WarrantyTypesId, W.Warranty, ASSMD.PhoneDetails AS PhoneDetails,
		ASSMD.PhoneManufacturer AS PhoneManufacturer,ASSMD.PhoneApiLevel AS PhoneApiLevel, ASSMD.AppVersion AS AppVersion, ASSMD.PhoneImei AS PhoneImei
		FROM AbSure_CarDetails ACD WITH(NOLOCK)
		LEFT JOIN AbSure_CarSurveyorMapping ACSM WITH(NOLOCK)ON ACD.Id = ACSM.AbSure_CarDetailsId 
		LEFT JOIN TC_Users TU WITH(NOLOCK)ON ACSM.TC_UserId = TU.Id 
		LEFT JOIN Cities C WITH(NOLOCK)ON ACD.OwnerCityId = C.ID
		LEFT JOIN AbSure_WarrantyTypes W WITH(NOLOCK) ON W.AbSure_WarrantyTypesId = ACD.AbSure_WarrantyTypesId
		LEFT JOIN AbSure_SaveSurveyorMobileDetails ASSMD ON ACD.Id = ASSMD.AbSure_CarId 
		WHERE ACD.Id = @AbSure_CarDetailsId	ORDER BY ASSMD.ID DESC

		SELECT @SurveyorId = TU.Id FROM AbSure_CarDetails ACD WITH(NOLOCK)
		LEFT JOIN AbSure_CarSurveyorMapping ACSM ON ACD.Id = ACSM.AbSure_CarDetailsId 
		LEFT JOIN TC_Users TU ON ACSM.TC_UserId = TU.Id   
		WHERE ACD.Id = @AbSure_CarDetailsId			

		EXEC TC_GetImmediateParent @SurveyorId, @TC_ReportingTo = @AgencyId OUTPUT

		SELECT CASE WHEN @IsAgency <> 1 THEN UserName 
		ELSE '' END AS AgencyName FROM TC_Users WITH(NOLOCk)
		WHERE Id = @AgencyId

		END				
		


-----------------------------------------------------------------------------


/****** Object:  StoredProcedure [dbo].[AbSure_GetCarFuelType]    Script Date: 8/5/2015 10:29:49 AM ******/
SET ANSI_NULLS ON
