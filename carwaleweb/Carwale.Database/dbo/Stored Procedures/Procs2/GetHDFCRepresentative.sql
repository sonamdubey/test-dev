IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetHDFCRepresentative]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetHDFCRepresentative]
GO

	-- =============================================
-- Author:		<kush kumar>
-- Create date: <20/9/2012>
-- Modified date: <4/10/2012>
-- Description:	<For HDFC Used Car Loan Implementation>
-- =============================================
CREATE PROCEDURE  [dbo].[GetHDFCRepresentative]

	@Representative VarChar(50) OUTPUT,
	@Mobile VarChar(10) OUTPUT,
	@Email VarChar(100) OUTPUT,
	@DealerId BigInt,
	@ASM VarChar(50) OUTPUT,
	@ASMEmailId VarChar(100) OUTPUT,
	@ASMContactNo VarChar(10) OUTPUT
 
	AS 
	BEGIN
		SELECT 
			@Representative = Representative,
			@Mobile = Mobile,
			@Email = Email,
			@ASM = ASM,
			@ASMEmailId = ASMEmailId,
			@ASMContactNo = ASMContactNo
		FROM HDFCDealerRepresentatives 
		WHERE DealerId = @DealerId;
	END    
