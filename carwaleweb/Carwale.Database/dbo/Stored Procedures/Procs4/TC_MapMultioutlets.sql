IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_MapMultioutlets]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_MapMultioutlets]
GO

	
-- =============================================
-- Author:		<vivek rajak>
-- Create date: <24/04/2015>
-- Description:	<To insert multioutlet form data to table TC_DealerAdminMaping>
-- =============================================
CREATE PROCEDURE [dbo].[TC_MapMultioutlets] 
	@DealerAdminId varchar(50),
	@DealerId varchar(50)

AS
BEGIN

	INSERT INTO TC_DealerAdminMapping(DealerAdminId,DealerId) 
	VALUES(@DealerAdminId,@DealerId);
END

