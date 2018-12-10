
GO
/****** Object:  StoredProcedure [dbo].[adv_FetchDealerDetails]    Script Date: 08-11-2016 18:45:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Harshil Mehta
-- Create date: 04-11-2016
-- Description:	fetch Dealer  details
-- =============================================
IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[adv_FetchDealerDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[adv_FetchDealerDetails]
GO
CREATE PROCEDURE [dbo].[adv_FetchDealerDetails]
 @dealerId INT
	
AS
BEGIN
select DD.ContactEmail as DealerEmail ,DD.ContactMobile as DealerMobile,D.Organization as DealerName,D.ContactPerson,D.Address1+D.Address2 as DealerAddress from TC_Deals_Dealers DD WITH (NOLOCK) 
inner join Dealers D WITH (NOLOCK) on d.ID = 5  where DD.DealerId = 5 AND DD.IsDealerDealActive = 1
End


GO


