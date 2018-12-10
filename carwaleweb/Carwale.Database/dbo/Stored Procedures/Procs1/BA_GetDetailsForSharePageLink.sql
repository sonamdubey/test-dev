IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BA_GetDetailsForSharePageLink]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BA_GetDetailsForSharePageLink]
GO

	-- =============================================
-- Author:		Ranjeet Kumar
-- Create date: 04-Jun-14
-- Description:	Get the full stock|broker details for the AutoBiz share page link. 
-- =============================================
CREATE PROCEDURE [dbo].[BA_GetDetailsForSharePageLink]
	@StockId INT,
	@DealerId INT = NULL,
	@IsCarWale BIT = NULL,
	@IsShowInterestData BIT =NULL  
AS
BEGIN
	SET NOCOUNT ON;
	IF @IsShowInterestData = 0
	BEGIN
			--Increase the Page View Count by 1
			UPDATE  BA_Stock SET [PageView] =  (SELECT (BS.PageView + 1) FROM  BA_Stock AS BS WHERE BS.ID = @StockId) WHERE ID = @stockId

			--Broker and Stock Details
			SELECT TOP 1  stockDetails.Color AS Color, stockDetails.Comments AS Comment,stockDetails.Kms AS Kms,stockDetails.Price AS Price,
			carTrasmission.Descr AS Transmission, carFuelType.FuelType AS FuelType, broker.BrokerName AS Name, broker.BrokerMobile AS Mobile, broker.Email AS Email,
			stockDetails.OwnerTypeId AS Owner,(makes.Name +' '+models.Name + ' '+ version.Name) AS CarName 
			FROM BA_StockDetails AS stockDetails WITH(NOLOCK)
			LEFT JOIN CarTransmission AS carTrasmission WITH(NOLOCK) ON carTrasmission.Id = stockDetails.TransmissionId AND stockDetails.IsActive =1
			LEFT JOIN CarFuelType AS carFuelType WITH(NOLOCK) ON carFuelType.FuelTypeId = stockDetails.FuelTypeId
			INNER JOIN CarVersions AS version WITH(NOLOCK) ON version.ID = stockDetails.CarVersionId
			INNER JOIN CarModels AS models WITH(NOLOCK) ON version.CarModelId = models.ID
			INNER JOIN CarMakes AS makes WITH(NOLOCK) ON makes.ID = models.CarMakeId  
			INNER JOIN BA_RegisterBroker AS broker WITH(NOLOCK) ON stockDetails.BrokerId = broker.ID   AND broker.IsActive = 1  
			WHERE stockDetails.StockId = @StockId
	END
	ELSE --@IsShowInterestData = 1
		BEGIN
			--Get the dealer Name,Mobile who has shown interest and Broker mobile
			IF @IsCarWale = 1 --Carwale
				BEGIN
					SELECT (D.FirstName + ' '+ D.LastName) AS DName, D.MobileNo AS DMobile, broker.BrokerMobile AS BMobile,
					(makes.Name+ ' '+models.Name) AS MakeModel,@IsCarWale AS Car
					FROM Dealers AS D WITH(NOLOCK)
					INNER JOIN BA_StockDetails AS stockDetails WITH(NOLOCK) ON  stockDetails.IsActive = 1 
					INNER JOIN CarVersions AS version WITH(NOLOCK) ON version.ID = stockDetails.CarVersionId
					INNER JOIN CarModels AS models WITH(NOLOCK) ON models.ID = version.CarModelId
					INNER JOIN CarMakes AS makes WITH(NOLOCK) ON makes.ID = models.CarMakeId
					INNER JOIN BA_RegisterBroker AS broker WITH(NOLOCK) ON stockDetails.BrokerId = broker.ID   AND broker.IsActive = 1  
					WHERE D.ID = @DealerId AND stockDetails.StockId = @StockId
				END
			ELSE --Non CarWale dealer
				BEGIN
					SELECT BNC.Name AS DName, BNC.Mobile AS DMobile, broker.BrokerMobile AS BMobile,(makes.Name+ ' '+models.Name) AS MakeModel ,@IsCarWale AS Car
					FROM BA_NonCarWaleDealer AS BNC WITH(NOLOCK) --WHERE BN.ID = @DealerId
					INNER JOIN BA_StockDetails AS stockDetails WITH(NOLOCK) ON  stockDetails.IsActive = 1 
					INNER JOIN CarVersions AS version WITH(NOLOCK) ON version.ID = stockDetails.CarVersionId
					INNER JOIN CarModels AS models WITH(NOLOCK) ON models.ID = version.CarModelId
					INNER JOIN CarMakes AS makes WITH(NOLOCK) ON makes.ID = models.CarMakeId
					INNER JOIN BA_RegisterBroker AS broker WITH(NOLOCK) ON stockDetails.BrokerId = broker.ID   AND broker.IsActive = 1  
					WHERE BNC.ID = @DealerId AND stockDetails.StockId = @StockId
				END
	END
	--Get the Stock Image row 
	SELECT imageSize.Small AS Thumb, imageSize.Large AS Large,imageSize.ID AS ImageId, (imageSize.HostUrl + imageSize.Dir) AS HostUrl
	FROM BA_ImageSize AS imageSize WITH(NOLOCK)
	INNER JOIN BA_StockImage AS stockImage WITH(NOLOCK) ON  stockImage.ID = imageSize.StockImageId 
														AND imageSize.StatusId = 3 AND imageSize.IsReplicated = 1 AND stockImage.IsActive = 1
														WHERE stockImage.StockId = @StockId

	

END

/****** Object:  StoredProcedure [dbo].[BA_GetUserContacts]    Script Date: 6/12/2014 7:49:51 PM ******/
SET ANSI_NULLS ON
