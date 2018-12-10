IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_DelateMaskingNumbers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_DelateMaskingNumbers]
GO

	-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <17/03/2015>
-- Description:	<Get Masking number against cityid>
-- Modifier   :  Mihir Chheda[12-08-2016]
-- Description:  log the masking number if removed or mark as invalid 
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_DelateMaskingNumbers]
	@MaskingNumber	VARCHAR(MAX),
	@IsValid BIT,
	@UpdatedBy INT
AS	
BEGIN
    DECLARE @TemptTable Table(ID INT)

	INSERT INTO @TemptTable(ID)
    SELECT ListMember FROM fnSplitCSV(@MaskingNumber)

    INSERT INTO MM_AvailableNumbersLog(MaskingNumber,StateId,CityId,ServiceProviderId,IsValid,UpdatedBy) -- Mihir Chheda[12-08-2016]
	SELECT MaskingNumber,StateId,CityId,ServiceProvider,@IsValid,@UpdatedBy FROM MM_AvailableNumbers(NOLOCK) 
	WHERE Id IN (SELECT Id FROM @TemptTable)

    DELETE FROM MM_AvailableNumbers 
	WHERE ID IN(SELECT Id FROM @TemptTable)
	
END


