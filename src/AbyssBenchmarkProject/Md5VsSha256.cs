﻿using AbyssBenchmarkLib;
using System;
using System.Security.Cryptography;

namespace AbyssBenchmarkProject;

public class Md5VsSha256
{
    private const int N = 10000;
    private readonly byte[] data;

    private readonly SHA256 sha256 = SHA256.Create();
    private readonly MD5 md5 = MD5.Create();

    public Md5VsSha256()
    {
        data = new byte[N];
        new Random(42).NextBytes(data);
    }

    [AbyssBenchmark]
    public byte[] Sha256() => sha256.ComputeHash(data);

    [AbyssBenchmark]
    public byte[] Md5() => md5.ComputeHash(data);
}