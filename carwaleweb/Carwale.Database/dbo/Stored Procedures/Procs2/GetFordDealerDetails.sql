IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetFordDealerDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetFordDealerDetails]
GO

	-- =============================================
-- Author:		Vinayak
-- Create date: 8/06/2016
-- Desc: To get ford dealers state and city according to their system
-- =============================================
CREATE PROCEDURE [dbo].[GetFordDealerDetails] 
	@DealerCode varchar(30)
AS
BEGIN
	SELECT State as StateName,City  as CityName
	FROM FordDealerDetails FD WITH (NOLOCK)
	where FD.Dealer_Code=@DealerCode
END