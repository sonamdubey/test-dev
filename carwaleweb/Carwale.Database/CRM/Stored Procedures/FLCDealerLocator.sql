IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[FLCDealerLocator]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[FLCDealerLocator]
GO

	--Summary	: Get FLC Dealer Locator
--Author	: Dilip V. 26-Oct-2012
--Modifier	: Amit Kumar 10 june 2013(Added constrain of target lead and deleted lead for removal of blocked dealer)

CREATE PROCEDURE [CRM].[FLCDealerLocator]
	@MakeId		NUMERIC(18,0),
	@StateId	NUMERIC(18,0),
	@CityId		NUMERIC(18,0) = NULL
 AS
	
BEGIN
	SET NOCOUNT ON	
	DECLARE	@varsql		VARCHAR(MAX)
	
		SELECT DS.Name Dealer, DS.Address, DS.Mobile, DS.LandlineNo, DS.EMail,C.Name City, DS.ContactPerson
		FROM Ncs_Dealers DS, NCS_DealerMakes DM, CarMakes AS CMA, Cities AS C
		WHERE DS.Id = DM.DealerId AND DM.MakeId = CMA.Id 
			 AND DM.MakeId =@MakeId AND DS.CityId = C.ID AND DS.IsActive = 1  
			 AND (CityId IN(SELECT Id FROM vwDealerCities WHERE CityId IN(SELECT ID FROM Cities WHERE StateId = @StateId)))
			 AND (CityId = @CityId OR @CityId IS NULL)
			 AND (ISNULL(Ds.TargetLeads,0) <> ISNULL(Ds.DelLeads,0) OR ISNULL(Ds.EndDate, GETDATE()) >= GETDATE())
			 AND Ds.ID NOT IN (SELECT DealerID FROM CRM.FLCDealerPriority WHERE ModelId=-1 AND Priority=2)
		ORDER BY C.Name,DS.Name
               
END









