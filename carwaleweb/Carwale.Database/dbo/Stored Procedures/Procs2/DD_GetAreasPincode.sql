IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DD_GetAreasPincode]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DD_GetAreasPincode]
GO

	
-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <12/11/2014>
-- Description:	<Get Pincode to auto suggest>
-- =============================================
CREATE PROCEDURE [dbo].[DD_GetAreasPincode]
@SearchText	VARCHAR(10)
AS
BEGIN
	SELECT DISTINCT PinCode FROM Areas WHERE PinCode LIKE  @SearchText + '%'
END
