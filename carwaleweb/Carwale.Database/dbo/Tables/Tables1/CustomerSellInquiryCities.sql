CREATE TABLE [dbo].[CustomerSellInquiryCities] (
    [SellInquiryId] NUMERIC (18)  NOT NULL,
    [CityId]        NUMERIC (18)  NOT NULL,
    [City]          VARCHAR (100) NULL,
    [EmailId]       VARCHAR (100) NULL,
    [PhoneNo]       VARCHAR (100) NULL,
    CONSTRAINT [PK_CustomerSellInquiryCities] PRIMARY KEY CLUSTERED ([SellInquiryId] ASC) WITH (FILLFACTOR = 90)
);


GO
-- =============================================
-- Author:		<Rajeev>
-- Create date: <3/10/08>
-- Description:	<For updating data of Live stock>
-- =============================================
CREATE TRIGGER [dbo].[TrigUpdateCustCarCityData] 
   ON  [dbo].[CustomerSellInquiryCities]
   FOR INSERT,UPDATE
AS 
DECLARE
	@ProfileId		AS VARCHAR(50), 
	@StateId		AS NUMERIC, 
	@StateName		AS VARCHAR(100),
	@CityId			AS NUMERIC, 
	@CityName		AS VARCHAR(100),
	@AreaId			AS NUMERIC, 
	@AreaName		AS VARCHAR(100),
	@Lattitude		AS DECIMAL(18,4), 
	@Longitude		AS DECIMAL(18,4)
BEGIN
	
	SELECT	
		@ProfileId = 'S' + CAST(I.SellInquiryId AS VarChar(50)),
		@StateId = S.Id,		@StateName = S.Name,		@CityId = C.Id,				
		@CityName = C.Name,		@Lattitude = C.Lattitude,	@Longitude = C.Longitude
	FROM 
		Inserted AS I, Cities AS C, States AS S
	WHERE
		I.CityId = C.Id AND S.ID = C.StateId

	--update the details in the live listings table
	Update 
		LiveListings 
	SET 
		StateId = @StateId,		StateName = @StateName,		CityId = @CityId,
		CityName = @CityName,	Lattitude = @Lattitude,		Longitude = @Longitude
	Where 
		ProfileId = @ProfileId
	

END
