IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CheckInactiveDealerCar]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CheckInactiveDealerCar]
GO

	
-- =============================================
-- Author:		Manish
-- Create date: 26-12-2012
-- Description:	Alert if any dealer set to inactive and their car is listed in carwale.
-- =============================================
CREATE PROCEDURE [dbo].[CheckInactiveDealerCar]	
AS
BEGIN
select D.ID AS [InActive DealerId],COUNT(*) as [Car Count on Live]
from LiveListings as LL      WITH (NOLOCK)
     join SellInquiries as S WITH (NOLOCK) on S.ID=LL.Inquiryid and LL.SellerType=1
     join Dealers as D       WITH (NOLOCK)       on D.ID=S.DealerId
where D.IsDealerActive=0
GROUP BY D.ID
end


