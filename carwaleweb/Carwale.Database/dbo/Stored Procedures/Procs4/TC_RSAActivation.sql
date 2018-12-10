IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_RSAActivation]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_RSAActivation]
GO

	-- ===============================================
-- Author:		Yuga Hatolkar
-- Create date: 07/04/15
-- Description:	Activate RSA
-- ===============================================
CREATE PROCEDURE [dbo].[TC_RSAActivation] 

@SoldRSAId INT,
@RSAActivation BIT,
@ActivatedBy INT

AS

	BEGIN	

		 UPDATE TC_SoldRSAPackages SET IsActivated = @RSAActivation, ActivationDate = GETDATE(), ActivatedBy = @ActivatedBy 
		 WHERE Id = @SoldRSAId		 

	END

