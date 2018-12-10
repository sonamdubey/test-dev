IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetMaskingNumberForDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetMaskingNumberForDealers]
GO

	-- =============================================
-- Author:		Akansha
-- Modified:    Akansha
-- Modification Date: 22 May 2013
-- Create date: 9-5-2013
-- Description:	Gets dealers name and active masking number 
-- Modified by Aditi Dhaybar on 25/09/2014
-- =============================================
CREATE PROCEDURE [dbo].[GetMaskingNumberForDealers] 
	@DealerID INT,
	@DealerName VARCHAR(200) OUTPUT,
	@MaskingNumber VARCHAR(15) OUTPUT,
	@MobileNo VARCHAR(10) OUTPUT		--Added by Aditi Dhaybar on 25/09/2014 for new contact seller functionality 
AS
BEGIN
	SELECT @DealerName = (d.FirstName + ' ' + d.LastName)
		,@MaskingNumber = d.ActiveMaskingNumber
		,@MobileNo = d.MobileNo
	FROM Dealers d WITH (NOLOCK)
	WHERE d.Id = @DealerID
END
