IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetMFCDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetMFCDealers]
GO
	--Author: Tejashree Patil on 31 Oct 2014
--Descripion: To get MFC dealers to push Inquiries.

CREATE PROCEDURE TC_GetMFCDealers
AS
BEGIN
    SELECT  DealerId
    FROM    TC_MFCDealers
END