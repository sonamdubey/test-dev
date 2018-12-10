IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_DealerDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_DealerDetails]
GO

	-- =============================================
-- Author:		Umesh Ojha
-- Create date: 16/4/2012
-- Description:	This SP returns the data of the dealer when Purchase inquiry by the customer insertde into thDB
-- =============================================
CREATE PROCEDURE [dbo].[Microsite_DealerDetails] 
	-- Add the parameters for the stored procedure here
	@DealerId BigInt
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select D.Organization AS SellerName, D.EmailId AS SellerEmail,
	(D.MobileNo + CASE WHEN D.PhoneNo IS NOT NULL AND D.PhoneNo <> '' THEN ', ' + D.PhoneNo ELSE NULL END) AS Contact,
	(D.Address1 +', '+ D.Address2) AS SellerAddress, D.ContactPerson 
	From Dealers AS D, Cities AS Ct, States AS St 
	WHERE D.ID=@DealerId AND D.CityId = Ct.ID AND Ct.StateId = St.ID
END
