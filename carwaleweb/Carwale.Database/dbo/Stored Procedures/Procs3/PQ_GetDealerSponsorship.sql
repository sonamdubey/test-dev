IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[PQ_GetDealerSponsorship]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[PQ_GetDealerSponsorship]
GO

	-- =============================================
-- Author:		Raghu
-- Create date: 31/12/2013
-- Description:	Get Sponsored dealer
-- Modified By Raghu : on <4/8/2014>  Added DealerEmailId Column
-- Modified By Vikas : on <7/7/2014>  added date between startdate and enddate clause, also Top 1 with order by newid, Added parametre ZoneId
-- Approved by Manish : on 14-07-2014 checked indexes
-- Modified By Vikas : on <11/8/2014>  modified the where clause for cityid and zoneid
--modified by ashish Verma Instead of dealer id we are retrieving dealer info by campaign id on 1/09/2014
-- =============================================
CREATE PROCEDURE [dbo].[PQ_GetDealerSponsorship] --[dbo].[PQ_GetDealerSponsorship] 28,1,null
	-- Add the parameters for the stored procedure here
	@ModelId NUMERIC,
	@CityId  INT,
	@ZoneId  INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Top 1 PDS.Id AS DealerId,PDS.DealerName,PDS.Phone AS PhoneNo,PDS.DealerEmailId AS DealerEmail,dl.DealerLeadBusinessType--modified by ashish Verma Instead of dealer id we are retrieving dealer info by campaign id on 1/09/2014
	FROM PQ_DealerSponsored  PDS WITH(NOLOCK)
	INNER JOIN PQ_DealerCitiesModels PCM  WITH(NOLOCK) ON PCM.PqId = PDS.Id
	INNER JOIN Dealers dl WITH(NOLOCK) on dl.ID = PDS.DealerId --modified by ashish Verma Instead of dealer id we are retrieving dealer info by campaign id on 1/09/2014
	WHERE	( (PCM.CityId=@cityid  AND ISNULL(PCM.ZoneId,0) =ISNULL(@ZoneId,0) --modified by vikas
                          )-- modified by vikas
                     OR PCM.CityId=-1
            )
	and PCM.ModelId=@ModelId and IsActive = 1-- modified by vikas
	and CONVERT(date,GETDATE()) BETWEEN  CONVERT(date,PDS.StartDate) and CONVERT(date,PDS.EndDate)-- modified by vikas
	ORDER BY dl.DealerLeadBusinessType, NEWID();-- modified by vikas

END



/****** Object:  StoredProcedure [dbo].[PQ_GetDealerSponsorshipV1.1]    Script Date: 9/4/2014 8:14:34 AM ******/
SET ANSI_NULLS ON
