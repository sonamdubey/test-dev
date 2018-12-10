IF EXISTS(
SELECT *
   FROM sys.views
     WHERE schema_id = SCHEMA_ID('dbo'))
     name = 'VIWLISTVERSION' AND
     DROP VIEW dbo.VIWLISTVERSION
GO

	
/*THIS IS THE VIEW FOR THE LIST OF SELLINQUIRIES ACCORDING TO THE
MODEL. IN THIS VIEW WE GET THE COMPLETE LIST OF THE CARS, UNDER THE
SELECTED MODEL. GET THE COMPLETE DETAILS
*/
CREATE VIEW VIWLISTVERSION AS 
SELECT
SI.ID AS ID, 
CV.Name AS Version, 
CV.CarModelId AS ModelId,
SI.Price AS Price, 
SI.Kilometers AS Kilometers, 
SI.Comments AS Description,
D.Organization AS Dealer,
D.LogoUrl AS DealerLogo,
D.ID AS DealerId
FROM
SellInquiries AS SI,
CarVersions AS CV,
Dealers AS D
WHERE
CV.ID = SI.CarVersionId AND
D.ID = SI.DealerId AND
SI.IsArchived = 0






