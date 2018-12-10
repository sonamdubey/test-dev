IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_GetMailContent]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_GetMailContent]
GO

	
-- =============================================================================
-- ALTERd By  : Suresh Prajapati on 07th May, 2015
-- Description : To get mail content parameters of warranty approval/rejection
-- EXEC Absure_GetMailContent 156
-- Updated By : Vinay Kumar Prajapati 28th May 2015  validation on  FinalWarrantyDate
-- Modified By : Suresh Prajapati on 15th June, 2015
-- Description : Fetched RegistraionType from Absure_RegistrationTypes table
-- =============================================================================
CREATE PROCEDURE [dbo].[Absure_GetMailContent] @AbsureCarId INT
AS
BEGIN
	SELECT ACD.OwnerName
		,ACD.OwnerPhoneNo
		,ACD.OwnerEmail
		,ACD.Make + ' ' + ACD.Model + ' ' + ACD.Version AS CarName
		,ACD.RegNumber
		,AWT.Warranty
		,ACD.StockId
		,ACD.Id AS CarId
		,DATEADD(DAY, 30, ISNULL(ACD.FinalWarrantyDate, GETDATE())) AS CertificateValidTill
		--,ACD.RegistrationType
		,RT.RegistrationTypeId AS RegistrationType
		,ACD.IsRejected
	FROM AbSure_CarDetails AS ACD WITH (NOLOCK)
	LEFT JOIN AbSure_WarrantyTypes AS AWT WITH (NOLOCK) ON AWT.AbSure_WarrantyTypesId = ACD.FinalWarrantyTypeId
	INNER JOIN Absure_RegistrationTypes AS RT WITH (NOLOCK) ON RT.RegistrationTypeId = ISNULL(ACD.RegistrationType, 1)
	WHERE ACD.Id = @AbsureCarId
		AND ACD.IsSurveyDone = 1
		AND ACD.IsActive = 1
		AND ISNULL(ACD.IsCancelled, 0) <> 1
END

