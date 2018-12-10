IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[UCAlert].[SetDailyNewlyAddedUsedCars]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [UCAlert].[SetDailyNewlyAddedUsedCars]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 26-12-2011
-- Description:	Newly Added used Cars in last week
-- Modified By : Ashish G. Kamble on 24/1/2012
-- Modified By: Manish on 07-08-2014 added two column CertificationId,RootId for Used car mailer.
-- Modified By: Manish on 19-08-2015 added try and exception block
-- =============================================
CREATE PROCEDURE [UCAlert].[SetDailyNewlyAddedUsedCars]
AS
BEGIN
   
   BEGIN TRY
    TRUNCATE TABLE UCAlert.NewlyAddedDailyCars
   
	INSERT INTO UCAlert.NewlyAddedDailyCars
	(
		ProfileId	,
		SellerType	,
		MakeId	,
		ModelId	,
		CityId	,
		MakeYear	,
		Price	,
		Kilometers	,
		EntryDate	,
		Make	,
		Model	,
		Version	,
		City	,
		Seller	,
		Color,
		HasPhoto,
		LastUpdated,
		BodyStyleId,
		FuelType,
		TransmissionId,
		CertificationId,
		RootId
	
	)
	SELECT  ProfileId,
			SellerType,
			MakeId,
			ModelId,
			CityId,
			year(MakeYear) as MakeYear,
			Price,
			Kilometers,
			EntryDate,
			MakeName,
			ModelName,
			VersionName,
			CityName,
			Seller,
			Color,
			(case isnull(PhotoCount,0) when 0 then 0 else 1 end ) HasPhoto,
			LastUpdated,
			CV.BodyStyleId,
			CV.CarFuelType,
			CV.CarTransmission	,
			LL.CertificationId,
			LL.RootId	
	FROM    LiveListings as LL WITH(NOLOCK)
	      JOIN CarVersions as CV on CV.ID=LL.VersionId
	WHERE EntryDate between GETDATE()-1 and GETDATE()

  END TRY
  BEGIN CATCH 
       INSERT INTO CarWaleWebSiteExceptions (
			                                ModuleName,
											SPName,
											ErrorMsg,
											TableName,
											FailedId,
											CreatedOn)
						            VALUES ('Used Car Daily alert email',
									        'UCAlert.SetDailyNewlyAddedUsedCars',
											 ERROR_MESSAGE(),
											 'UCAlert.NewlyAddedDailyCars',
											 NULL,
											 GETDATE()
                                            )
  
  
  
  END CATCH 	  

END


