import matplotlib.pyplot as plt
import numpy as np

words = []
real = []
expected = []

with open('Results/graph_bi_data.txt', 'r', encoding='utf-8') as f:
    for line in f:
        parts = line.replace(',', '.').strip().split(' ')
        words.append(parts[0])
        real.append(float(parts[1]))
        expected.append(float(parts[2]))

words = words[:100]
real = real[:100]
expected = expected[:100]

n = len(words)
width = 0.35
x = np.arange(n)
figsize = (25, 15) 

fig, ax = plt.subplots(figsize=figsize)

rects1 = ax.bar(x - width/2, real, width, 
               label='Реальная частота', 
               color='#006699',
               alpha=0.8)

rects2 = ax.bar(x + width/2, expected, width, 
               label='Ожидаемая частота', 
               color='#ff6600',
               alpha=0.8)

ax.set_title('Сравнение частот биграмм (топ-100)', fontsize=18, pad=20)
ax.set_ylabel('Частота', fontsize=14)
ax.set_xticks(x)
ax.set_xticklabels(
    words,
    rotation=90,
    ha='center',
    va='top',
    fontsize=9,
    fontfamily='DejaVu Sans'
)
for xtick in x:
    ax.axvline(xtick, color='gray', alpha=0.1, linestyle='--')

ax.legend(fontsize=12, loc='upper right')
ax.grid(axis='y', alpha=0.3)
ax.set_xlim(-0.5, n-0.5)

plt.savefig('Results/gen-1.png')

words = []
real = []
expected = []

with open('Results/graph_word_data.txt', 'r', encoding='utf-8') as f:
    for line in f:
        parts = line.replace(',', '.').strip().split(' ')
        words.append(parts[0])
        real.append(float(parts[1]))
        expected.append(float(parts[2]))

words = words[:100]
real = real[:100]
expected = expected[:100]

n = len(words)
width = 0.35
x = np.arange(n)
figsize = (25, 15) 

fig, ax = plt.subplots(figsize=figsize)

rects1 = ax.bar(x - width/2, real, width, 
               label='Реальная частота', 
               color='#006699',
               alpha=0.8)

rects2 = ax.bar(x + width/2, expected, width, 
               label='Ожидаемая частота', 
               color='#ff6600',
               alpha=0.8)

ax.set_title('Сравнение частот слов (топ-100)', fontsize=18, pad=20)
ax.set_ylabel('Частота', fontsize=14)
ax.set_xticks(x)
ax.set_xticklabels(
    words,
    rotation=90,
    ha='center',
    va='top',
    fontsize=9,
    fontfamily='DejaVu Sans'
)
for xtick in x:
    ax.axvline(xtick, color='gray', alpha=0.1, linestyle='--')

ax.legend(fontsize=12, loc='upper right')
ax.grid(axis='y', alpha=0.3)
ax.set_xlim(-0.5, n-0.5)

plt.savefig('Results/gen-2.png')
