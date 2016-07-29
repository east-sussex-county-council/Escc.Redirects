CREATE TABLE [dbo].[Redirect] (
    [RedirectId]  INT           IDENTITY (1, 1) NOT NULL,
    [Pattern]     VARCHAR (200) NOT NULL,
    [Type]        INT           NOT NULL,
    [Comment]     VARCHAR (200) NULL,
    [DateCreated] DATETIME      CONSTRAINT [DF_Redirects_DateCreated] DEFAULT (getdate()) NOT NULL,
    [Destination] VARCHAR (400) NULL
);

