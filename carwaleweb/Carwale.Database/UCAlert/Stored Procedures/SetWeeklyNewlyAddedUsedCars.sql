IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[UCAlert].[SetWeeklyNewlyAddedUsedCars]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [UCAlert].[SetWeeklyNewlyAddedUsedCars]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 26-12-2011
-- Description:	Newly Added used Cars in last week
-- Modified By : Ashish G. Kamble on 24/1/2012
-- Modified By: Manish on 07-08-2014 added two column CertificationId,RootId for Used car mailer.
-- Modified By: Manish on 26-08-2015 added try and exception block
-- =============================================
CREATE PROCEDURE [UCAlert].[SetWeeklyNewlyAddedUsedCars]
AS
BEGIN
   
    BEGIN TRY
    TRUNCATE TABLE UCAlert.NewlyAddedWeeklyCars
   
	INSERT INTO UCAlert.NewlyAddedWeeklyCars
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
		LastUpdated	,
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
			LastUpdated	,
			CertificationId,
		     RootId
	FROM    LiveListings
	WHERE EntryDate between GETDATE()-7 and GETDATE()
  END TRY
  BEGIN CATCH 
       INSERT INTO CarWaleWebSiteExceptions (
			                                ModuleName,
											SPName,
											ErrorMsg,
											TableName,
											FailedId,
											CreatedOn)
						            VALUES ('Used Car Weekly alert email',
									        'UCAlert.SetWeeklyNewlyAddedUsedCars',
											 ERROR_MESSAGE(),
											 'UCAlert.NewlyAddedWeeklyCars',
											 NULL,
											 GETDATE()
                                            )
  
  
  
  END CATCH 	   

END


