IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_SaveActivatedPkg]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_SaveActivatedPkg]
GO

	-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <23/04/2015>
-- Description:	<Save Activated package for dealer>
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_SaveActivatedPkg] 
	@DealerId	INT,
    @UpdatedBy	INT
AS
BEGIN
	INSERT INTO DCRM_ActivatedPackages (DealerId,UpdatedBy)values(@DealerId , @UpdatedBy)
END
