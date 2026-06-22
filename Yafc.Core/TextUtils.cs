using System.Text;

namespace Yafc.Core;

public static class TextUtils {
    public static string NormalizeHangulCompatibilityJamo(string value) {
        if (string.IsNullOrEmpty(value)) {
            return value;
        }

        StringBuilder result = new(value.Length);

        for (int i = 0; i < value.Length; i++) {
            int initial = GetInitialIndex(value[i]);

            if (initial >= 0 && i + 1 < value.Length && TryReadVowel(value, i + 1, out int vowel, out int vowelLength)) {
                int final = 0;
                int finalLength = 0;
                int finalPosition = i + 1 + vowelLength;

                if (finalPosition < value.Length) {
                    int firstFinal = GetFinalIndex(value[finalPosition]);

                    if (firstFinal > 0 && !IsVowelStart(value, finalPosition + 1)) {
                        if (finalPosition + 1 < value.Length
                            && TryCombineFinal(firstFinal, GetFinalIndex(value[finalPosition + 1]), out int combinedFinal)
                            && !IsVowelStart(value, finalPosition + 2)) {
                            final = combinedFinal;
                            finalLength = 2;
                        }
                        else {
                            final = firstFinal;
                            finalLength = 1;
                        }
                    }
                }

                _ = result.Append((char)(0xAC00 + ((initial * 21 + vowel) * 28) + final));
                i += vowelLength + finalLength;
            }
            else {
                _ = result.Append(value[i]);
            }
        }

        return result.ToString().Normalize(NormalizationForm.FormC);
    }

    private static bool TryReadVowel(string value, int position, out int vowel, out int length) {
        vowel = GetVowelIndex(value[position]);
        length = 1;

        if (vowel < 0) {
            return false;
        }

        if (position + 1 < value.Length && TryCombineVowel(vowel, GetVowelIndex(value[position + 1]), out int combinedVowel)) {
            vowel = combinedVowel;
            length = 2;
        }

        return true;
    }

    private static bool IsVowelStart(string value, int position) => position < value.Length && GetVowelIndex(value[position]) >= 0;

    private static int GetInitialIndex(char value) => value switch {
        '\u3131' => 0,
        '\u3132' => 1,
        '\u3134' => 2,
        '\u3137' => 3,
        '\u3138' => 4,
        '\u3139' => 5,
        '\u3141' => 6,
        '\u3142' => 7,
        '\u3143' => 8,
        '\u3145' => 9,
        '\u3146' => 10,
        '\u3147' => 11,
        '\u3148' => 12,
        '\u3149' => 13,
        '\u314A' => 14,
        '\u314B' => 15,
        '\u314C' => 16,
        '\u314D' => 17,
        '\u314E' => 18,
        _ => -1
    };

    private static int GetVowelIndex(char value) => value switch {
        '\u314F' => 0,
        '\u3150' => 1,
        '\u3151' => 2,
        '\u3152' => 3,
        '\u3153' => 4,
        '\u3154' => 5,
        '\u3155' => 6,
        '\u3156' => 7,
        '\u3157' => 8,
        '\u3158' => 9,
        '\u3159' => 10,
        '\u315A' => 11,
        '\u315B' => 12,
        '\u315C' => 13,
        '\u315D' => 14,
        '\u315E' => 15,
        '\u315F' => 16,
        '\u3160' => 17,
        '\u3161' => 18,
        '\u3162' => 19,
        '\u3163' => 20,
        _ => -1
    };

    private static int GetFinalIndex(char value) => value switch {
        '\u3131' => 1,
        '\u3132' => 2,
        '\u3133' => 3,
        '\u3134' => 4,
        '\u3135' => 5,
        '\u3136' => 6,
        '\u3137' => 7,
        '\u3139' => 8,
        '\u313A' => 9,
        '\u313B' => 10,
        '\u313C' => 11,
        '\u313D' => 12,
        '\u313E' => 13,
        '\u313F' => 14,
        '\u3140' => 15,
        '\u3141' => 16,
        '\u3142' => 17,
        '\u3144' => 18,
        '\u3145' => 19,
        '\u3146' => 20,
        '\u3147' => 21,
        '\u3148' => 22,
        '\u314A' => 23,
        '\u314B' => 24,
        '\u314C' => 25,
        '\u314D' => 26,
        '\u314E' => 27,
        _ => -1
    };

    private static bool TryCombineVowel(int first, int second, out int combined) {
        combined = (first, second) switch {
            (8, 0) => 9,
            (8, 1) => 10,
            (8, 20) => 11,
            (13, 4) => 14,
            (13, 5) => 15,
            (13, 20) => 16,
            (18, 20) => 19,
            _ => -1
        };

        return combined >= 0;
    }

    private static bool TryCombineFinal(int first, int second, out int combined) {
        combined = (first, second) switch {
            (1, 19) => 3,
            (4, 22) => 5,
            (4, 27) => 6,
            (8, 1) => 9,
            (8, 16) => 10,
            (8, 17) => 11,
            (8, 19) => 12,
            (8, 25) => 13,
            (8, 26) => 14,
            (8, 27) => 15,
            (17, 19) => 18,
            _ => -1
        };

        return combined >= 0;
    }
}
