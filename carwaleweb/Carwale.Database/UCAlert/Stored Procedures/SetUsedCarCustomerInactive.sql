IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[UCAlert].[SetUsedCarCustomerInactive]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [UCAlert].[SetUsedCarCustomerInactive]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 26-12-2011
-- Description:	Set Customer Inactive 
-- Modified By: Manish on 29 July 2015 changed table from UCAlert.UserCarAlerts to [UCAlert].[NDUsedCarAlertCustomerList]
-- =============================================
CREATE PROCEDURE [UCAlert].[SetUsedCarCustomerInactive](@UsedCarAlertId int)
AS
BEGIN
   
    UPDATE [UCAlert].[NDUsedCarAlertCustomerList] --UCAlert.UserCarAlerts
    SET IsActive=0
    WHERE  UsedCarAlert_Id=@UsedCarAlertId

END
