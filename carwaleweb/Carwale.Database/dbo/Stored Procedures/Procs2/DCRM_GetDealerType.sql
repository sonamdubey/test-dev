IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetDealerType]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetDealerType]
GO

	
-- =============================================
-- Author	:	Sachin Bharti(3rd June 2015)
-- Description	:	Get dealer type 
-- Modified By:Komal Manjare(08-07-2016)
-- Remove isactive condition(show all values)
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetDealerType]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT 
		TD.TC_DealerTypeId AS Id,
		TD.DealerType 
	FROM
		TC_DealerType TD(NOLOCK) 
	-- WHERE
	--	TD.IsActive = 1  -- commented by:Komal Manjare(08-07-2016) Remove isactive condition
    ORDER BY
		TD.DealerType 
END
