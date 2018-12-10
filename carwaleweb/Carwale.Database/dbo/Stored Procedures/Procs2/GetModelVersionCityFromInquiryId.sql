IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetModelVersionCityFromInquiryId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetModelVersionCityFromInquiryId]
GO

	-- =============================================
-- Author:		Vicky Gupta
-- Description:	<Get the Vesion AND Model of a car of a given Inquiry Id>
-- Create On:  <15/12/2015>
-- GetModelVersionFromInquiryId 10483
-- Modified by : Khushaboo Patil on 26 oct 2016 to fetch model year
-- =============================================
CREATE PROCEDURE [dbo].[GetModelVersionCityFromInquiryId] 
(

	@InquiryId INT	
	
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @ModelId NUMERIC  -- data type numric is taken bcoz of table CarModels
	DECLARE @VersionId NUMERIC
	DECLARE @CityId NUMERIC
	DECLARE @ModelYear INT

	SELECT @VersionId =  NI.VersionId,@CityId = NI.CityId,@ModelYear = ModelYear
	FROM TC_NewCarInquiries NI WITH(NOLOCK)
	LEFT JOIN TC_NewCarBooking CB WITH(NOLOCK) ON CB.TC_NewCarInquiriesId = NI.TC_NewCarInquiriesId
	WHERE 
	NI.TC_NewCarInquiriesId=@InquiryId; 

	SELECT @ModelId = CV.CarModelId
	FROM CarVersions CV WITH(NOLOCK)
	WHERE
	CV.ID = @VersionId

	SELECT @VersionId AS VersionId,@ModelId AS ModelId,@CityId AS CityId , @ModelYear ModelYear


END