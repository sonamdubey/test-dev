IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetDealerType]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetDealerType]
GO

	
-- =============================================
-- Author	    :	Vicky Gupta(29th Dec 2015)
-- Description	:  To get dealer type in string format ex-UCD for a given dealer id
-- TC_GetDealerType 5
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetDealerType]
@DealerId INT,
@DealerType VARCHAR(50)=NULL OUT
AS
       BEGIN
	        SET @DealerType = (SELECT DT.DealerType from Dealers AS AD WITH(NOLOCK) 
								INNER JOIN TC_DealerType AS DT WITH(NOLOCK) ON AD.ID=@DealerId  AND AD.TC_DealerTypeId = DT.TC_DealerTypeId)					
	
        END
--------------------------------------------------------------------------------------------------------------------
SET ANSI_NULLS ON
