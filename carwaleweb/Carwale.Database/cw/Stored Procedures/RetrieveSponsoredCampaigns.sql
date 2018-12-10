IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[RetrieveSponsoredCampaigns]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[RetrieveSponsoredCampaigns]
GO

	-- =============================================      
-- Author:  Prashant Vishe      
-- Create date: 11-Mar-2013      
-- Description: To fetch all the sposored campigns    
-- Modified :By Prashant Vishe On 21 may 2013            
--Modification:removed condition of where isDeleted=0 
-- Modified :By Prashant Vishe On 30 aug 2013           
--Modification:added condition of where isDeleted=0
-- =============================================      
CREATE PROCEDURE [cw].[RetrieveSponsoredCampaigns]      
AS      
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;      
      
    SELECT * FROM SponsoredCampaigns WITH (NOLOCK)      
 WHERE IsActive=1 AND IsDeleted=0 AND GETDATE() BETWEEN StartDate AND EndDate   ---removed condition of where isDeleted=0 
  order by StartDate desc  ---added order by starDate in query   
END
