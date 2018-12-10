IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DD_GetDealersName]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DD_GetDealersName]
GO

	
-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <16/10/2014>
-- Description:	<Get DealerNames>
-- =============================================
CREATE PROCEDURE [dbo].[DD_GetDealersName]
@DealerId		 INT,
@DealerName		 VARCHAR(100) OUTPUT
AS
BEGIN

	SELECT @DealerName = Name from DD_DealerNames WHERE ID = @DealerId 

END

