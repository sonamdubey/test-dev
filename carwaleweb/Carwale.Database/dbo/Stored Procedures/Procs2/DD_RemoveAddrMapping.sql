IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DD_RemoveAddrMapping]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DD_RemoveAddrMapping]
GO

	


-------------------------------------------------------------------------------------------


-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <27/11/2014>
-- Description:	<Remove Address Mapping with dealer outlet>
-- =============================================
CREATE PROCEDURE [dbo].[DD_RemoveAddrMapping]
	@OutletId	INT
AS
BEGIN
	DELETE FROM DD_DealerOutletAddress WHERE DD_DealerOutletsId = @OutletId
END

