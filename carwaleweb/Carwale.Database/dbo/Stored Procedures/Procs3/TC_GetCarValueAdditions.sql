IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetCarValueAdditions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetCarValueAdditions]
GO
	

-- =============================================
-- Author:		<Nilesh Utture>
-- Create date: <21/12/2012>
-- Description:	<Get car value additions for API>
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetCarValueAdditions]

AS
BEGIN
		SELECT TC_CarValueAdditionsId AS Id,ValueAddName AS Name FROM TC_CarValueAdditions WHERE IsActive=1
END



/****** Object:  StoredProcedure [dbo].[TC_GetCertificationOrganization]    Script Date: 12/24/2012 16:13:52 ******/
SET ANSI_NULLS ON

